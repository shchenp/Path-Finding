public interface IHighlightable
{
    bool IsObstacle { get; }

    void HighlightFinalPoint();

    void HighlightOnPath();

    void SetColor(bool isAvailable);

    void ResetColor();
    
    
}