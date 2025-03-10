using MassTransit;

namespace SagaStateMachine.Service.StateMachines
{
    public class OrderStateMachine : MassTransitStateMachine<OrderStateInstance>
    {

        public OrderStateMachine()
        {
            InstanceState(Instance => Instance.CurrentState);
        }

    }
}
