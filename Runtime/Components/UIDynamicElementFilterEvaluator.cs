namespace GourdUI
{
    public static class UIDynamicElementFilterEvaluator
    {
        public static void EvaluateFilter(UIDynamicElement rect, IUIDynamicElementFilter filter)
        {
            switch (filter)
            {
                case IUIDynamicElementPositionFilter positionFilter:
                    rect.ApplyPositionFilterResult(
                        positionFilter.GetFilteredPosition());
                    break;
            }
        }
    }
}