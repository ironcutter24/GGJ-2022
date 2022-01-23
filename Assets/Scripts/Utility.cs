using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
	public class Util
	{
		public static Vector3Int ScreenSize { get { return Vector3Int.right * Screen.width + Vector3Int.up * Screen.height; } }

		public static Vector3 ScreenCenter { get { return Vector3.right * ScreenSize.x * .5f + Vector3.up * ScreenSize.y * .5f; } }

		public static bool Chance(float chanceOfSuccess)
		{
			return Random.Range(0f, 1f) <= chanceOfSuccess;
		}
	}

	public class UMath
	{
		public static float Normalize(float value, float min, float max)
		{
			return (value - min) / (max - min);
		}
		
		public static float DivideByZero(float dividend, float divider)
		{
			try
			{
				return dividend / divider;
			}
			catch (System.DivideByZeroException)
			{
				return float.PositiveInfinity;
			}
		}

		public static float Vector2ToAngle(Vector2 direction)
		{
			return Vector2.SignedAngle(Vector2.right, direction);
		}

		public static Vector2 AngleToVector2(float angle)
		{
			Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
			return direction.normalized;
		}
		
		public static float DistanceXZ(Vector3 a, Vector3 b) { return Mathf.Sqrt(SqrDistanceXZ(a, b)); }

		public static float SqrDistanceXZ(Vector3 a, Vector3 b) { return Mathf.Pow((a.x - b.x), 2) + Mathf.Pow((a.z - b.z), 2); }

		public static Vector3 GetDirectionXZ(Vector3 fromPosition, Vector3 targetPosition)
		{
			return GetXZ(targetPosition - fromPosition).normalized;
		}

		public static Vector3 GetXZ(Vector3 source)
        {
			return Vector3.right * source.x + Vector3.forward * source.z;
        }

		public static Vector3 AxisProduct(Vector3 a, Vector3 b)
		{
			return NewVector(a.x * b.x, a.y * b.y, a.z * b.z);
		}

		public static Vector3 NewVector(float x, float y, float z)
		{
			return Vector3.right * x + Vector3.up * y + Vector3.forward * z;
		}
	}

	class OneTimeEvent
	{
		bool performed = false;

		System.Func<bool> Condition;
		System.Action Callback;

		public OneTimeEvent(System.Func<bool> condition, System.Action callback)
		{
			this.Condition = condition;
			this.Callback = callback;
		}

		public void Evaluate()
		{
			if (!performed && Condition())
			{
				Callback();
				performed = true;
			}
		}

		public void Reset()
		{
			performed = false;
		}
	}

	namespace Patterns
	{
		public class Singleton<T> : MonoBehaviour where T : Component
		{
			protected static T _instance;
			public static T Instance { get { return _instance; } }

			protected virtual void Awake()
			{
				if (_instance == null)
					_instance = this as T;
				else
					Destroy(this.gameObject);
			}
		}
		
		namespace FSM
		{
			public abstract class State<T>
			{
				protected T user;

				protected State(T user)
				{
					this.user = user;
				}

				public virtual void Enter() { }

				public abstract void Process();

				public virtual void Exit() { }
			}
		}
	}
}
