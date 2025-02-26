namespace Game.Player.States
{
    public sealed class PlayerCaughtState : PlayerState
    {
        public override void Initialize()
        {
            _gameManager.FirePlayerCaught();

            _player.View.NavMeshStatus(false);
            _player.View.Idle();
        }

        public override void Dispose()
        {

        }
    }
}