namespace Rocket.RocketLoader.Patches
{
    public class ItemAsset : Patch
    {
        private PatchHelper h = new PatchHelper("SDG.ItemAsset");

        public void Apply()
        {
            h.UnlockFieldByType(typeof(string), "Name", 0);
            h.UnlockFieldByType(typeof(string), "Description", 1);
            h.UnlockFieldByType("EItemType", "ItemType");
            h.UnlockFieldByType("EUseableType", "UseableType");
            h.UnlockFieldByType("ESlotType", "SlotType");
            h.UnlockFieldByType("GameObject", "GameObject");
            h.UnlockFieldByType("AudioClip", "AudioClips");
            h.UnlockFieldByType("AnimationClip[]", "AnimationClips");
            h.UnlockFieldByType("Blueprint[]", "Blueprints");
            h.UnlockFieldByType(typeof(byte), "SizeX");
            h.UnlockFieldByType(typeof(byte), "SizeY", 1);
            h.UnlockFieldByType(typeof(float), "SizeZ");
            h.UnlockFieldByType(typeof(byte), "Amount", 2);


        }
    }
}