namespace Rocket.RocketLoader.Unturned.Patches
{
    [Class("SDG.ItemAsset")]
    public class ItemAsset : Patch
    {
        public override void Apply()
        {
            UnlockFieldByType(typeof(string), "Name", 0);
            UnlockFieldByType(typeof(string), "Description", 1);
            UnlockFieldByType("EItemType", "ItemType");
            UnlockFieldByType("EUseableType", "UseableType");
            UnlockFieldByType("ESlotType", "SlotType");
            UnlockFieldByType("GameObject", "GameObject");
            UnlockFieldByType("AudioClip", "AudioClips");
            UnlockFieldByType("AnimationClip[]", "AnimationClips");
            UnlockFieldByType("Blueprint[]", "Blueprints");
            UnlockFieldByType(typeof(byte), "SizeX");
            UnlockFieldByType(typeof(byte), "SizeY", 1);
            UnlockFieldByType(typeof(float), "SizeZ");
            UnlockFieldByType(typeof(byte), "Amount", 2);
            UnlockFieldByType(typeof(bool), "Cosmetic");
        }
    }
}