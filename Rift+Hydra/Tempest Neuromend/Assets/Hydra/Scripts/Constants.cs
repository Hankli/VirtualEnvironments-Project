namespace Tempest
{
	namespace RazorHydra
	{
		public enum Hands
		{
			UNKNOWN = 0,
			LEFT = 1,
			RIGHT = 2
		}

		public enum Buttons
		{
			START = 1,
			ONE = 32,
			TWO = 64,
			THREE = 8,
			FOUR = 16,
			BUMPER = 128,
			JOYSTICK = 256,
			TRIGGER = 512,
		}

		public enum ControllerManagerState
		{
			NONE = 0,
			BIND_CONTROLLER_ONE = 1,
			BIND_CONTROLLER_TWO = 2
		}
	}
}