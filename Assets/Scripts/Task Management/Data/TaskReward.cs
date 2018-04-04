// Author:			Wolfgang Neumayer
// Creation Date:	04/04/2018

namespace game.taskmanagement.data
{
    using game.taskmanagement.enumerations;

    [System.Serializable]
    public class TaskReward
    {
        public TaskRewardType type;
        public float amount;
    }
}