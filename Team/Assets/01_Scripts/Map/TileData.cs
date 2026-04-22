/// <summary>
/// 1. 타일 1칸의 정보를 가지고 있음
/// </summary>
public enum TileType
{
    Grass,
    Water,

}

public class TileData 
{    
    public int x;
    public int z;    

    public int height = 1;

    public TileType tileType;

    public bool buildable;  // 건물을 지을 수 있는가
    public bool occupied;   // 이미 건물이 있는가
}
