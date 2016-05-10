using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalChessProject.BoardSettings
{
    public abstract class Move
    {

        private Tuple<int, int> movePosition;
        public abstract bool isAttackMove();
        public Move(Tuple<int, int> movePosition)
        {
            this.movePosition = movePosition;
        }
        public Tuple<int,int> getMovePosition()
        {
            return this.movePosition;
        }
        //    public abstract bool isLegalMove(Tuple<int, int> currrentPosition, Tuple<int, int> destinationPosition);
    }
    public class AttackMove : Move
    {
        public AttackMove(Tuple<int, int> movePosition) : base(movePosition) { }
        public override bool isAttackMove()
        {
            return true;
        }
    }
    public class NormalMove : Move
    {
        public NormalMove(Tuple<int, int> movePosition) : base(movePosition) { }
        public override bool isAttackMove()
        {
            return false;
        }
    }
}
