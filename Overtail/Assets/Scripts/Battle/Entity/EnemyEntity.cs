namespace Overtail.Battle.Entity
{
    /// <summary>
    /// Base combat related enemy class.
    /// Derive from this class for any monsters or use <see cref="GenericEnemyEntity"/>.
    /// </summary>
    public class EnemyEntity : BattleEntity
    {
        public int Affection { get; protected set; }
    }
}
