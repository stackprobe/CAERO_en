using System.Globalization;
using System.Numerics;
using Raylib_cs;

namespace Caero;

internal sealed record WarpTerminal(int Dx, int Dz, int Distance, Vector3 LocalPosition, Vector3 LocalFacing)
{
    internal string Direction => (Dx, Dz) switch {
        (0, -1) => "North", (1, -1) => "Northeast", (1, 0) => "East", (1, 1) => "Southeast",
        (0, 1) => "South", (-1, 1) => "Southwest", (-1, 0) => "West", _ => "Northwest"
    };
    internal string Letters => (Dz < 0 ? "N" : Dz > 0 ? "S" : "") + (Dx < 0 ? "W" : Dx > 0 ? "E" : "");
    internal string ConfirmationMessage => "Warp to a destination "
        + string.Join(" and ", new[] {
            Dz == 0 ? null : $"{Distance} islands {(Dz < 0 ? "north" : "south")}",
            Dx == 0 ? null : $"{Distance} islands {(Dx < 0 ? "west" : "east")}"
        }.Where(part => part != null)) + " away?";
    internal bool Destination(long x, long z, out long targetX, out long targetZ)
    {
        Int128 a = (Int128)x + Dx * Distance, b = (Int128)z + Dz * Distance;
        targetX = targetZ = 0;
        if (a < long.MinValue || a > long.MaxValue || b < long.MinValue || b > long.MaxValue) return false;
        targetX = (long)a; targetZ = (long)b; return true;
    }
}

internal sealed partial class Sector
{
    internal int FloatingCourtFloors { get; private set; }
    internal int? WarpFloor { get; private set; }
    internal List<WarpTerminal> WarpTerminals { get; } = [];
    internal DestinationPanel? WarpNumberPanel { get; private set; }
    internal Vector3 WarpArrivalPosition => Transform(WarpFloor is { } level
        ? new(60, HighRoof + level * 12, -50) : new(20, TowerFloors * 12, 22));

    private void BuildWarpZone()
    {
        if (FloatingCourtFloors == 0) return;
        // This stream is independent of architecture, plants and residents.
        var random = new Xor128(SeedAt(worldSeed ^ 0x57415250, X, Z));
        WarpFloor = random.Next(FloatingCourtFloors + 1); // Base floor through roof, inclusive.
        Vector3 center = new(60, HighRoof + WarpFloor.Value * 12, -50);
        WarpNumberPanel = new(IslandNumber, center, Unrotate(-Vector3.UnitZ), maxWidth: 6, maxHeight: 6, underline: true);
        ApplyWayfindingInlay(WarpNumberPanel.Footprint, WarpNumberPanel.Surface);
        foreach (var (dx, dz) in new[] { (0, -1), (1, -1), (1, 0), (1, 1), (0, 1), (-1, 1), (-1, 0), (-1, -1) })
        {
            int distance = random.Next(dx == 0 || dz == 0 ? 30 : 20, dx == 0 || dz == 0 ? 71 : 51);
            Vector3 outward = Unrotate(Vector3.Normalize(new Vector3(dx, 0, dz)));
            var terminal = new WarpTerminal(dx, dz, distance, center + outward * 7.3f, -outward);
            WarpTerminals.Add(terminal);
            Stone.Append(BuildWarpTerminal(terminal));
        }
        Route("warp room approach", new(50, center.Y, -59.5f), new(54.5f, center.Y, -59.5f),
            new(57.2f, center.Y, -56.8f), center);
    }

    private Geometry BuildWarpTerminal(WarpTerminal terminal)
    {
        var g = new Geometry();
        Vector3 p = terminal.LocalPosition, n = terminal.LocalFacing, right = Vector3.Cross(Vector3.UnitY, n);
        float yaw = MathF.Atan2(n.X, n.Z);
        Color metal = new(91, 116, 126, 253), edge = new(154, 175, 178, 253);
        g.Box(p + Vector3.UnitY * .2f, new(1.5f, .4f, 1.3f), edge, yaw);
        g.Box(p + Vector3.UnitY * 1.1f, new(1.15f, 1.4f, .8f), metal, yaw);
        g.Cylinder(p + Vector3.UnitY * 1.8f, .12f, .09f, .48f, edge, 8);
        g.Ellipsoid(p + Vector3.UnitY * 2.32f, new(.18f), new(142, 212, 224, 253), 8, 4);
        foreach (float side in new[] { -1f, 1f })
            g.Box(p + right * (side * .48f) + n * .43f + Vector3.UnitY * .61f,
                new(.12f, .18f, .10f), edge, yaw);
        var pixels = new int[23, 21];
        Stamp(pixels, terminal.Letters, (23 - (terminal.Letters.Length * 6 - 1)) / 2, 2, 1);
        Stamp(pixels, terminal.Distance.ToString(CultureInfo.InvariantCulture), 8, 12, 2);
        for (int x = 5; x < 18; x++) pixels[x, 18] = 1;
        Vector3 panel = p + n * .4f + Vector3.UnitY * 1.15f;
        InlayPixels(pixels, .043f, (u, v) => panel + right * u + Vector3.UnitY * v,
            [new(25, 44, 53, 253), new(159, 207, 221, 253), new(231, 226, 185, 253)], g);
        g.RemoveCoplanarOverlaps();
        return g;
    }
}

internal sealed record WarpTarget(Sector Sector, WarpTerminal Terminal)
{
    internal Vector3 Point => Sector.Transform(Terminal.LocalPosition + Terminal.LocalFacing * .58f + Vector3.UnitY * 1.15f);
}

internal static class WarpTravel
{
    internal const float ArrivalDrop = 1.1f;
    internal static bool Examine(World world, Explorer player, WarpTarget target, Conversation conversation, Action? beforeTravel = null)
    {
        if (player.Flying) return false;
        if (!target.Terminal.Destination(target.Sector.X, target.Sector.Z, out _, out _)) return false;
        conversation.Confirm("Warp Device", target.Terminal.ConfirmationMessage, () => {
            beforeTravel?.Invoke();
            Travel(world, player, target);
        });
        return true;
    }
    internal static WarpTarget? Target(World world, Vector3 eye, Vector3 forward)
    {
        WarpTarget? closest = null; float best = 3.4f * 3.4f;
        foreach (var sector in world.Sectors)
            foreach (var terminal in sector.WarpTerminals)
            {
                var target = new WarpTarget(sector, terminal);
                Vector3 delta = target.Point - eye;
                float distance = delta.LengthSquared();
                if (distance >= best || distance < .00001f || Vector3.Dot(Vector3.Normalize(delta), forward) <= .35f) continue;
                Vector3 facing = sector.Transform(terminal.LocalFacing) - sector.Offset;
                if (Vector3.Dot(-delta, facing) <= 0) continue;
                if (world.Sectors.Any(s => s.Collision.Obstructs(eye, target.Point))) continue;
                closest = target; best = distance;
            }
        return closest;
    }

    internal static bool Travel(World world, Explorer player, WarpTarget target)
    {
        if (!target.Terminal.Destination(target.Sector.X, target.Sector.Z, out long x, out long z)) return false;
        Vector3 forward = Vector3.Normalize(new Vector3(target.Terminal.Dx, 0, target.Terminal.Dz));
        return world.BeginWarp(x, z, forward);
    }
}
