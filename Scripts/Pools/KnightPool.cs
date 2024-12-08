using System.Linq;
using UnityEngine;

public class KnightPool : Pool<Knight>
{
    public void OrderToMove(Vector3 target)
    {
        Knight knight = ObjectsPool.FirstOrDefault
            (knight => knight.IsBusy == false);

        if (knight != null)
        {
            KnightMover knightMover = knight.GetComponent<KnightMover>();

            if (knightMover != null)
            {
                knightMover.GoToTarget(target);
            }
        }
    }

    public bool IsFreeKnight()
    {
        return ObjectsPool.Count - ObjectsPool.Count(knight => knight.IsBusy) > 0;
    }
}
