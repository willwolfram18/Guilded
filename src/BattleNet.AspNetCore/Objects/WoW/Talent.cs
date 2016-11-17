namespace BattleNetApi.Objects.WoW
{
    public class Talent
    {
        public TalentGridCoordinates TalentGridCoorindates { get; private set; }

        public Spell Spell { get; private set; }

        public Specialization SpecTalentBelongsTo { get; private set; }
    }
}
