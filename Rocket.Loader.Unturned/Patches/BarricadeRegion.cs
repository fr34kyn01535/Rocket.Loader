namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.Unturned.BarricadeRegion")]
    public class BarricadeRegion : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType("List<Transform>", "Barricades");
            UnlockFieldByType("List<BarricadeData>", "BarricadeDatas");
        }
    }
}