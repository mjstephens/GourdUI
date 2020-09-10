namespace GourdUI
{
    public static class UIDynamicRectFilterEvaluator
    {
        public static void EvaluateFilter(UIDynamicRect rect, IUIDynamicRectFilter filter)
        {
            switch (filter)
            {
                case IUIDynamicRectPositionFilter positionFilter:
                    rect.ApplyPositionFilterResult(
                        positionFilter.GetFilteredPosition());
                    break;
            }
        }
    }
}