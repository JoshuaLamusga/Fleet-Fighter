namespace FleetFighter
{
    /// <summary>
    /// Represents an enemy classification.
    /// </summary>
    public enum EnemyType
    {
        /// <summary>
        /// First enemy type. Flies straight down at a slow speed and dies in
        /// one hit.
        /// </summary>
        Flyer,

        /// <summary>
        /// Second enemy type. Flies straight down, then banks.
        /// </summary>
        BankingFlyer,

        /// <summary>
        /// Third enemy type. Flies down and stops to shoot, then continues.
        /// </summary>
        Sentry,

        /// <summary>
        /// Fourth enemy type. Flies diagonally and wraps around the screen.
        /// </summary>
        Distractor,

        /// <summary>
        /// Fifth enemy type. Shoots at regular intervals and accelerates
        /// forward.
        /// </summary>
        ThrustFlyer,

        /// <summary>
        /// Sixth enemy type. A more powerful version of type two.
        /// </summary>
        BankingFlyerHard,

        /// <summary>
        /// Seventh enemy type. Flies straight down to hit the player.
        /// </summary>
        Bullet,

        /// <summary>
        /// Flies straight down and shoots type six enemies at intervals.
        /// </summary>
        Spawner
    }
}