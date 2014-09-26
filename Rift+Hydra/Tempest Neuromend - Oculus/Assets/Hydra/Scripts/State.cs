namespace Tempest
{
	public interface State<T>
	{
		void Enter(T entity);
		void Execute(T entity);
		void Exit(T entity);
	}

	public class FSM<T>
	{
		private T m_entity;
		private State<T> m_cState;
		private State<T> m_gState;

		public FSM(T entity)
		{ 
			m_entity = entity;
			m_cState = null;
		}

		public void Update(T entity)
		{
			if(m_gState != null)
				m_gState.Execute(m_entity);
		

			if(m_cState != null)
				m_cState.Execute(m_entity);

		}

		public void SwitchBackground(State<T> state)
		{
			if(m_gState != null) 
				m_gState.Exit (m_entity);
			
			m_gState = state;
			
			m_gState.Enter (m_entity);
		}

		public void SwitchLocal(State<T> state)
		{
			if(m_cState != null)
				m_cState.Exit (m_entity);

			m_cState = state;

			m_cState.Enter (m_entity);
		}

	}
}
