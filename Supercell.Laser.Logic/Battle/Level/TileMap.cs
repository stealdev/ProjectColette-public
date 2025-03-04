﻿namespace Supercell.Laser.Logic.Battle.Level
{
    using System;
    using System.Collections.Generic;
    using System.Reflection.Emit;
    using System.Runtime.Intrinsics.Arm;
    using Newtonsoft.Json.Bson;
    using Supercell.Laser.Logic.Battle.Objects;
    using Supercell.Laser.Logic.Util;
    using Supercell.Laser.Titan.Debug;
    using Supercell.Laser.Titan.Math;

    public static class MapLoader
    {
        public static string[] InitWithMapFromDataTable(LogicRandom a2, DataTable dataTable, string name)
        {
            if (dataTable.Count < 1) return null;
            for (int i = 0; i < dataTable.Count; i++)
            {
                LogicData mapData = dataTable.GetData<LogicData>(i);
                if (mapData.GetCSVRow().GetValueAt(0) == name)
                {
                    if (mapData.GetCSVRow().GetArraySizeAt(1) > 0)
                    {
                        string[] mapdata = new string[mapData.GetCSVRow().GetArraySizeAt(1)];
                        int j = 0;
                        do
                        {
                            mapdata[j] = mapData.GetCSVRow().GetValueAt(1, j);
                            j++;
                        }
                        while (j < mapdata.Length);
                        return mapdata;
                    }
                }
            }
            return null;
        }
    }
    public class TileMap
    {
        public readonly int Width, Height;
        private readonly Tile[,] Tiles;

        public List<Tile> NewTiles;

        public int LogicWidth => TileToLogic(Width);
        public int LogicHeight => TileToLogic(Height);

        public List<Tile> SpawnPoints;
        public List<Tile> SpawnPointsTeam1;
        public List<Tile> SpawnPointsTeam2;

        public List<Tile> SpawnPointsBases;


        public TileMap(int width, int height, string data)
        {
            Width = width;
            Height = height;

            SpawnPoints = new List<Tile>();
            SpawnPointsTeam1 = new List<Tile>();
            SpawnPointsTeam2 = new List<Tile>();

            SpawnPointsBases = new List<Tile>();

            NewTiles = new List<Tile>();

            char[] chars = data.ToCharArray();
            int idx = 0;

            Tiles = new Tile[Height, Width];
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Tiles[i, j] = new Tile(chars[idx], TileToLogic(j), TileToLogic(i));
                    if (chars[idx] == '1')
                    {
                        SpawnPoints.Add(Tiles[i, j]);
                        SpawnPointsTeam1.Add(Tiles[i, j]);
                    }
                    else if (chars[idx] == '2')
                    {
                        SpawnPoints.Add(Tiles[i, j]);
                        SpawnPointsTeam2.Add(Tiles[i, j]);
                    }
                    else if (chars[idx] == '8')
                    {
                        SpawnPointsBases.Add(Tiles[i, j]);
                    }

                    idx++;
                }
            }
            if (!SpawnPointsTeam1.Any()) SpawnPointsTeam1.Add(Tiles[10, 10]);
        }
        public void Tick(GameObjectManager GameObjectManager)
        {
            for (int i = 0; i < NewTiles.Count; i++)
            {
                if (NewTiles[i].LifeTime > 0)
                {
                    NewTiles[i].LifeTime--;
                    if (NewTiles[i].LifeTime == 0) NewTiles[i].Destruct();
                }
                if (NewTiles[i].IsDestructed()) NewTiles.RemoveAt(i--);
            }
        }
        public bool IsPassable(int a2, int x, int y, int a5, int a6)
        {
            Tile tile = GetTile(x, y, true);
            if (tile == null) return false;
            return !tile.Data.BlocksMovement || tile.IsDestructed();
        }
        public bool CalculateLOSWithCollision(
        int a2,
        int a3,
        int a4,
        int a5,
        int a6)
        {
            int v10; // r9
            int v12; // [sp+18h] [bp-28h]
            int v13; // [sp+1Ch] [bp-24h]
            int v14; // [sp+20h] [bp-20h]

            v14 = LogicMath.Clamp(a6 + a4, 1, LogicWidth - 2);
            v13 = LogicMath.Clamp(a4 - a6, 1, LogicWidth - 2);
            v12 = LogicMath.Clamp(a6 + a5, 1, LogicHeight - 2);
            v10 = LogicMath.Clamp(a5 - a6, 1, LogicHeight - 2);
            //a2 += (a4 - a2) / LogicMath.Sqrt((a4 - a2) * (a4 - a2) + (a5 - a3) * (a5 - a3))
            if (!GamePlayUtil.GetClosestWorldCollision(a2, a3, a4, a5, this, new LogicVector2(), 0, 0, 0, 0)
              || !GamePlayUtil.GetClosestWorldCollision(a2, a3, v14, a5, this, new LogicVector2(), 0, 0, 0, 0)
              || !GamePlayUtil.GetClosestWorldCollision(a2, a3, v13, a5, this, new LogicVector2(), 0, 0, 0, 0)
              || !GamePlayUtil.GetClosestWorldCollision(a2, a3, a4, v12, this, new LogicVector2(), 0, 0, 0, 0))
            {
                return true;
            }
            //else if (!GamePlayUtil.GetClosestWorldCollision(a2, a3, a4, a5 - a6 / 2, this, new LogicVector2(), 0, 0, 0, 0)
            //  || !GamePlayUtil.GetClosestWorldCollision(a2, a3, a4 + a6 / 2, a5, this, new LogicVector2(), 0, 0, 0, 0)
            //  || !GamePlayUtil.GetClosestWorldCollision(a2, a3, a4 - a6 / 2, a5, this, new LogicVector2(), 0, 0, 0, 0)
            //  || !GamePlayUtil.GetClosestWorldCollision(a2, a3, a4, a5 + a6 / 2, this, new LogicVector2(), 0, 0, 0, 0))
            //{
            //    return true;
            //}
            else
            {
                return !GamePlayUtil.GetClosestWorldCollision(a2, a3, a4, v10, this, new LogicVector2(), 0, 0, 0, 0);
            }
        }
        public bool LineIntersectRectangle(int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9)
        {
            int v9; // w23
            int v10; // w8
            int v11; // w9
            int v12; // w25
            int v13; // w26
            double v19; // d0
            double v20; // d1
            int v21; // w9
            int v22; // w8

            v9 = a4 - a2;
            if (a4 >= a2)
                v10 = a4;
            else
                v10 = a2;
            if (a4 >= a2)
                v11 = a2;
            else
                v11 = a4;
            if (v10 <= a8)
                v12 = v10;
            else
                v12 = a8;
            if (v11 >= a6)
                v13 = v11;
            else
                v13 = a6;
            if (v13 > v12)
                return false;
            if (LogicMath.Abs(v9) >= 1)
            {
                v19 = (double)(a5 - a3) / (double)v9;
                v20 = (double)a3 - v19 * (double)a2;
                a3 = (int)(v20 + v19 * (double)v13);
                a5 = (int)(v20 + v19 * (double)v12);
            }
            if (a3 <= a5)
                v21 = a5;
            else
                v21 = a3;
            if (a3 <= a5)
                v22 = a3;
            else
                v22 = a5;
            if (v21 > a9)
                v21 = a9;
            if (v22 < a7)
                v22 = a7;
            return v22 <= v21;
        }
        public bool IsPlayerLineOfSightAlmostClear(
        int a2,
        int a3,
        int a4,
        int a5,
        int a6)
        {
            int v10; // r9
            int v12; // [sp+18h] [bp-28h]
            int v13; // [sp+1Ch] [bp-24h]
            int v14; // [sp+20h] [bp-20h]

            v14 = LogicMath.Clamp(a6 + a4, 1, LogicWidth - 2);
            v13 = LogicMath.Clamp(a4 - a6, 1, LogicWidth - 2);
            v12 = LogicMath.Clamp(a6 + a5, 1, LogicHeight - 2);
            v10 = LogicMath.Clamp(a5 - a6, 1, LogicHeight - 2);
            if (IsPlayerLineOfSightClear(a2, a3, a4, a5, false, false, false, false)
              || IsPlayerLineOfSightClear(a2, a3, v14, a5, false, false, false, false)
              || IsPlayerLineOfSightClear(a2, a3, v13, a5, false, false, false, false)
              || IsPlayerLineOfSightClear(a2, a3, a4, v12, false, false, false, false))
            {
                return true;
            }
            else
            {
                return IsPlayerLineOfSightClear(a2, a3, a4, v10, false, false, false, false);
            }
        }
        public bool IsPlayerLineOfSightClear(
        int a2,
        int a3,
        int a4,
        int a5,
        bool a6,
        bool a7,
        bool a8,
        bool a9)
        {
            int v11; // r4
            int v12; // r1
            int result; // r0
            int v14; // r4
            int v15; // r9
            bool v16; // cc
            int v17; // r10
            int v18; // r7
            bool v19; // cc
            int v20; // r7
            int v21; // r4
            int v22; // r5
            int v23; // r1
            double v24; // d8
            int v25; // r0
            double v26; // d9
            int v27; // r0
            int v28; // r8
            int v29; // r2
            int v30; // r0
            int v31; // r0
            int v32; // r9
            Tile v33; // r5
            int v34; // r4
            int v35; // r4
            int v36; // r5
            int v37; // r4
            int v38; // r12
            int v39; // r0
            Tile v40; // r0
            int v41; // r4
            int v42; // r3
            int v43; // r6
            int v44; // r10
            int v45; // r0
            Tile v46; // r0
            int v47; // r7
            int v48; // r4
            int v49; // r7
            int v50; // r0
            Tile v51; // r0
            int v52; // r4
            int v53; // r0
            int v54; // r4
            bool v55; // r0
            int v56; // r7
            int v57; // r0
            int v58; // r1
            Tile v59; // r0
            int v60; // r5
            int v61; // r4
            int v62; // r0
            int v63; // r4
            int v64; // r1
            int v65; // r4
            int v66; // r0
            int v67; // [sp+0h] [bp-70h]
            int v68; // [sp+4h] [bp-6Ch]
            int v69; // [sp+8h] [bp-68h]
            int v70; // [sp+Ch] [bp-64h]
            int v71; // [sp+Ch] [bp-64h]
            int v72; // [sp+10h] [bp-60h]
            int v74; // [sp+18h] [bp-58h]
            int v75; // [sp+1Ch] [bp-54h]
            int v77; // [sp+20h] [bp-50h]
            int v78; // [sp+24h] [bp-4Ch]
            int v79; // [sp+24h] [bp-4Ch]
            int v80; // [sp+24h] [bp-4Ch]
            int v81; // [sp+28h] [bp-48h]
            int v82; // [sp+2Ch] [bp-44h]
            int v83; // [sp+30h] [bp-40h]
            int v85; // [sp+38h] [bp-38h]
            int v86; // [sp+3Ch] [bp-34h]

            v11 = (int)((458129845L * a2) >> 32);
            v12 = Width;
            result = 0;
            v14 = (v11 >> 5) + (v11 >> 31);
            if (v14 < v12)
            {
                v15 = a4 / 300;
                if (a4 / 300 >= v12)
                    return false;
                v16 = a3 < -299;
                if (a3 >= -299)
                    v16 = a2 < -299;
                if (v16)
                    return false;
                v17 = a3 / 300;
                v18 = Height;
                if (a3 / 300 >= v18)
                    return false;
                v19 = a5 < -299;
                if (a5 >= -299)
                    v19 = a4 < -299;
                if (v19)
                {
                    return false;
                }
                else
                {
                    result = 0;
                    if (a5 / 300 < v18)
                    {
                        v20 = LogicMath.Min(v14, v15);
                        v21 = LogicMath.Max(v14, v15);
                        v22 = LogicMath.Min(v17, a5 / 300);
                        v85 = LogicMath.Max(v17, a5 / 300);
                        if (v20 > v21)
                            return true;
                        v23 = a4;
                        v72 = a4 - a2;
                        v24 = (double)(a5 - a3) / (double)(a4 - a2);
                        v69 = 300 * v22;
                        v25 = a2;
                        if (a4 < a2)
                        {
                            v25 = a4;
                            v23 = a2;
                        }
                        v75 = v25;
                        v77 = v23;
                        v68 = v21;
                        v67 = v22;
                        v26 = (double)a3 - v24 * (double)a2;
                        while (true)
                        {
                            if (v22 > v85)
                            {
                                v27 = v20 + 1;
                                goto LABEL_19;
                            }
                            v28 = v69;
                            v86 = v20;
                            v81 = 300 * v20;
                            v83 = v20 + 1;
                            v74 = v20 - 1;
                            do
                            {
                                v31 = Width;
                                v32 = v22;
                                v33 = null;
                                if (v31 > v20 && (v32 | v20) >= 0 && Height > v32) v33 = GetTile(v20, v32, true);
                                if (a7)
                                {
                                    //v34 = *v33;
                                    //if (!ZNK13LogicTileData14blocksMovementEv(*v33) || ZNK13LogicTileData17blocksProjectilesEv(v34))
                                    //    goto LABEL_32;
                                    //LABEL_44:
                                    //if (ZNK13LogicTileData26isDestructibleWithPiercingEv(v34))
                                    //    goto LABEL_32;
                                    //goto LABEL_57;
                                }
                                if (a6)
                                {
                                    //v34 = *v33;
                                    //if (!ZNK13LogicTileData14blocksMovementEv(*v33) || !ZNK13LogicTileData17blocksProjectilesEv(v34))
                                    //    goto LABEL_32;
                                    //goto LABEL_44;
                                }
                            //v35 = v33.BlocksVision();
                            //if (a8 && ZNK13LogicTileData17isDestructibleAnyEv(*v33))
                            //{
                            //    if (ZNK13LogicTileData17isDestructibleAnyEv(*v33) && !ZNK13LogicTileData17blocksProjectilesEv(*v33))
                            //    {
                            //        if (ZNK13LogicTileData14blocksMovementEv(*v33))
                            //            v35 = 0;
                            //    }
                            //    else
                            //    {
                            //        v35 = 0;
                            //    }
                            //}
                            //if (a9)
                            //{
                            //    if ((v35 & (ZNK13LogicTileData14getDynamicCodeEv(*v33) < 1)) == 0)
                            //        goto LABEL_32;
                            //}
                            //else if (!v35)
                            //{
                            //    goto LABEL_32;
                            //}
                            LABEL_57:
                                v36 = v33.Data.CollisionMargin;
                                v37 = v86;
                                v38 = v36 + 1;
                                v39 = Width;
                                if (v39 >= v86 && (v32 | v74) >= 0 && Height > v32 && GetTile(v74, v32, true) != null)
                                {
                                    v40 = GetTile(v74, v32, true);
                                    if (a6)
                                    {
                                        if (!v40.Data.BlocksMovement
                                          || !v40.Data.BlocksProjectiles
                                          || v40.Data.IsDestructible)
                                        {
                                        LABEL_87:
                                            v38 = v36 + 1;
                                            v42 = v36 + v81;
                                            v37 = v86;
                                            goto LABEL_67;
                                        }
                                    }
                                    else if (!v40.BlocksVision())
                                    {
                                        v38 = v36 + 1;
                                        v42 = v36 + v81;
                                        v37 = v86;
                                        goto LABEL_67;
                                    }
                                    v38 = v36 + 1;
                                    v37 = v86;
                                    v42 = v81 - 1;
                                }
                                else
                                {
                                    v42 = v81 - 1;
                                }
                            LABEL_67:
                                v43 = v36 + 300 * v32;
                                v44 = v36 + v81 - 2 * v36 + 300;
                                v45 = Width;
                                if (v45 > v83 && (v32 | v83) >= 0 && Height > v32)
                                {
                                    v46 = GetTile(v83, v32, true);
                                    if (v46 != null)
                                    {
                                        v47 = v42;
                                        v78 = v38;
                                        if (a6)
                                        {
                                            //v48 = *v46;
                                            //if (ZNK13LogicTileData14blocksMovementEv(*v46)
                                            //  && ZNK13LogicTileData17blocksProjectilesEv(v48)
                                            //  && !ZNK13LogicTileData26isDestructibleWithPiercingEv(v48))
                                            //{
                                            //LABEL_75:
                                            //    v38 = v78;
                                            //    v44 += v78;
                                            //LABEL_90:
                                            //    v37 = v86;
                                            //    v42 = v47;
                                            //    goto LABEL_77;
                                            //}
                                        }
                                        else if (v46.BlocksVision())
                                        {
                                            v38 = v78;
                                            v44 += v78;
                                            v37 = v86;
                                            v42 = v47;
                                            goto LABEL_77;
                                        }
                                        v38 = v78;
                                        v37 = v86;
                                        v42 = v47;
                                        goto LABEL_77;
                                    }
                                }
                                v44 += v38;
                            LABEL_77:
                                v49 = v43 - 2 * v36;
                                v50 = Width;
                                if (v50 <= v37)
                                {
                                    v82 = v43 - v38;
                                    goto LABEL_93;
                                }
                                if (((v32 - 1) | v37) < 0)
                                {
                                    v82 = v43 - v38;
                                    goto LABEL_93;
                                }
                                if (Height < v32)
                                {
                                    v82 = v43 - v38;
                                    goto LABEL_93;
                                }
                                v51 = GetTile(v37, v32 - 1, true);
                                if (v51 == null)
                                {
                                    v82 = v43 - v38;
                                    goto LABEL_93;
                                }
                                v82 = v36 + v28;
                                v79 = v38;
                                if (a6)
                                {
                                    //v52 = *v51;
                                    //v70 = v42;
                                    //if (!ZNK13LogicTileData14blocksMovementEv(*v51) || !ZNK13LogicTileData17blocksProjectilesEv(v52))
                                    //{
                                    //    v38 = v79;
                                    //    v37 = v86;
                                    //    v42 = v70;
                                    //    goto LABEL_93;
                                    //}
                                    //v53 = ZNK13LogicTileData26isDestructibleWithPiercingEv(v52);
                                    //v38 = v79;
                                    //v37 = v86;
                                    //v42 = v70;
                                    //if (v53)
                                    //    goto LABEL_93;
                                    //LABEL_92:
                                    //v82 = v43 - v38;
                                    //goto LABEL_93;
                                }
                                v54 = v42;
                                v55 = v51.BlocksVision();
                                v42 = v54;
                                v37 = v86;
                                v38 = v79;
                                if (v55)
                                {
                                    v82 = v43 - v38;
                                    goto LABEL_93;
                                }
                            LABEL_93:
                                v56 = v49 + 300;
                                v57 = Width;
                                v58 = v32 + 1;
                                v59 = GetTile(v37, v58, true);
                                if (v57 <= v37
                                  || ((v32 + 1) | v37) < 0
                                  || Height <= v58
                                  || GetTile(v37, v58, true) == null)
                                {
                                    v60 = v56 + v38;
                                    goto LABEL_103;
                                }
                                v80 = v38;
                                v60 = v28 - v36 + 300;
                                if (!a6)
                                {
                                    v65 = v42;
                                    v42 = v65;
                                    if (!v59.BlocksVision())
                                        goto LABEL_103;
                                    v60 = v56 + v80;
                                    goto LABEL_103;
                                }
                            //v71 = v42;
                            //v61 = *v59;
                            //if (ZNK13LogicTileData14blocksMovementEv(*v59) && ZNK13LogicTileData17blocksProjectilesEv(v61))
                            //{
                            //    v62 = ZNK13LogicTileData26isDestructibleWithPiercingEv(v61);
                            //    v42 = v71;
                            //    if (v62)
                            //        goto LABEL_103;
                            //    LABEL_111:
                            //    v60 = v56 + v80;
                            //    goto LABEL_103;
                            //}
                            //v42 = v71;
                            LABEL_103:
                                if (v75 >= v42)
                                    v42 = v75;
                                if (v77 <= v44)
                                    v44 = v77;
                                if (v42 <= v44)
                                {
                                    v63 = v42;
                                    v16 = LogicMath.Abs(v32) < 1;
                                    v30 = a3;
                                    v64 = a5;
                                    if (!v16)
                                    {
                                        v64 = (int)(v26 + v24 * (double)v44);
                                        v30 = (int)(v26 + v24 * (double)v63);
                                    }
                                    v29 = v64;
                                    if (v30 > v64)
                                        v29 = v30;
                                    if (v29 > v60)
                                        v29 = v60;
                                    if (v30 > v64)
                                        v30 = v64;
                                    if (v30 < v82)
                                        v30 = v82;
                                    Debugger.Print(v29.ToString());
                                    if (v30 <= v29)
                                        return false;
                                }
                            LABEL_32:
                                v28 += 300;
                                v20 = v86;
                                v22 = v32 + 1;
                            }
                            while (v32 < v85);
                            v21 = v68;
                            v22 = v67;
                            v27 = v83;
                        LABEL_19:
                            v16 = v20 < v21;
                            v20 = v27;
                            if (!v16)
                                return true;
                        }
                    }
                }
            }
            return false;
        }
        //public bool IsPlayerLineOfSightClear(
        //int a2,
        //int a3,
        //int a4,
        //int a5,
        //bool a6,
        //bool a7,
        //bool a8,
        //bool a9)
        //{
        //    int v9; // r4
        //    int result; // r0
        //    int v12; // r9
        //    int v13; // r6
        //    bool v14; // cc
        //    int v15; // r8
        //    int v16; // r5
        //    bool v17; // cc
        //    int v18; // r10
        //    int v19; // r5
        //    int v20; // r4
        //    int v21; // r6
        //    int v22; // r0
        //    int v23; // r2
        //    int v24; // r5
        //    bool v25; // r8
        //    _DWORD* v26; // r3
        //    int v27; // r1
        //    int v28; // r9
        //    bool v29; // r6
        //    int* v30; // r0
        //    Tile v31; // r0
        //    int* v32; // r0
        //    int* v33; // r0
        //    int v34; // r2
        //    int v35; // r3
        //    int v36; // r6
        //    int v37; // r0
        //    int v38; // r10
        //    Tile v39; // r4
        //    bool v40; // cc
        //    int v41; // r0
        //    int v42; // r0
        //    bool v43; // cc
        //    int v44; // r0
        //    bool v45; // cc
        //    int v46; // r0
        //    int v47; // r2
        //    bool v48; // cc
        //    int v49; // r0
        //    int v50; // r1
        //    int v51; // r2
        //    bool v52; // cc
        //    int v53; // [sp+18h] [bp-58h]
        //    int v54; // [sp+1Ch] [bp-54h]
        //    int v55; // [sp+20h] [bp-50h]
        //    int v56; // [sp+30h] [bp-40h]
        //    int v57; // [sp+34h] [bp-3Ch]
        //    int v61; // [sp+48h] [bp-28h]
        //    int v62; // [sp+4Ch] [bp-24h]

        //    v9 = Width;
        //    result = 0;
        //    v12 = a2 / 300;
        //    if (a2 / 300 < v9)
        //    {
        //        v13 = a4 / 300;
        //        if (a4 / 300 < v9)
        //        {
        //            v14 = a3 < -299;
        //            if (a3 >= -299)
        //                v14 = a2 < -299;
        //            if (!v14)
        //            {
        //                v15 = a3 / 300;
        //                v16 = Height;
        //                if (a3 / 300 < v16)
        //                {
        //                    v17 = a5 < -299;
        //                    if (a5 >= -299)
        //                        v17 = a4 < -299;
        //                    if (!v17)
        //                    {
        //                        v18 = a5 / 300;
        //                        if (a5 / 300 < v16)
        //                        {
        //                            v19 = LogicMath.Min(v12, a4 / 300);
        //                            v20 = LogicMath.Max(v12, v13);
        //                            v21 = LogicMath.Min(v15, v18);
        //                            v22 = LogicMath.Max(v15, v18);
        //                            v23 = v19;
        //                            v24 = v22;
        //                            result = 1;
        //                            if (v23 <= v20)
        //                            {
        //                                v25 = a7;
        //                                v54 = v20;
        //                                v53 = v21;
        //                                v57 = v24;
        //                                v55 = 300 * v21;
        //                                while (v21 > v24)
        //                                {
        //                                    v27 = v23 + 1;
        //                                LABEL_80:
        //                                    v14 = v23 < v20;
        //                                    v23 = v27;
        //                                    if (!v14)
        //                                        return result;
        //                                }
        //                                v28 = v55;
        //                                v62 = v23;
        //                                v61 = v23 + 1;
        //                                v56 = v23 - 1;
        //                                while (true)
        //                                {
        //                                    v37 = Width;
        //                                    v38 = v21;
        //                                    v39 = null;
        //                                    if (v37 > v23)
        //                                    {
        //                                        v40 = (v21 | v23) <= 0;
        //                                        if ((v21 | v23) >= 0)
        //                                            v40 = Width <= v21;
        //                                        if (!v40)
        //                                            v39 = GetTile(v21, v23, true);
        //                                    }
        //                                    if (v25)
        //                                        break;
        //                                    if (a6)
        //                                    {
        //                                    //    v41 = ShouldDestruct(*v39);
        //                                    //LABEL_54:
        //                                    //    if (!v41)
        //                                    //        goto LABEL_78;
        //                                    //    goto LABEL_55;
        //                                    }
        //                                    v29 = v39.BlocksVision();
        //                                    if (a8)
        //                                        //v29 &= sub_7BA338(v39) ^ 1;
        //                                    if (a9)
        //                                    {
        //                                        //if ((v29 & (ZNK13LogicTileData14getDynamicCodeEv(*v39) < 1)) == 0)
        //                                        //    goto LABEL_78;
        //                                    }
        //                                    else if (!v29)
        //                                    {
        //                                        goto LABEL_78;
        //                                    }
        //                                LABEL_55:
        //                                    v35 = v62;
        //                                    v42 = Width;
        //                                    if (v42 < v62)
        //                                        v34 = v61;
        //                                    v43 = (v38 | v56) <= 0;
        //                                    if ((v38 | v56) >= 0)
        //                                        v43 = Height <= v38;
        //                                    if (v43 || GetTile(v38,v56) == null)
        //                                    {
        //                                    LABEL_60:
        //                                        v34 = v61;
        //                                    }
        //                                    else
        //                                    {
        //                                        //if (a6)
        //                                        //    //ShouldDestruct(*v30);
        //                                        //else
        //                                        //    //sub_2DD460(v30);
        //                                        v34 = v61;
        //                                        v35 = v62;
        //                                    }
        //                                    v44 = Width;
        //                                    if (v44 > v34)
        //                                    {
        //                                        v45 = (v38 | v34) <= 0;
        //                                        if ((v38 | v34) >= 0)
        //                                            v45 = Height <= v38;
        //                                        if (!v45)
        //                                        {
        //                                            v31 = *(*v36 + 4 * (v34 + v44 * v38));
        //                                            if (v31!=null)
        //                                            {
        //                                                if (a6)
        //                                                    ShouldDestruct(*v31);
        //                                                else
        //                                                    sub_2DD460(v31);
        //                                                v35 = v62;
        //                                                v36 = a1;
        //                                            }
        //                                        }
        //                                    }
        //                                    v46 = *(v36 + 96);
        //                                    if (v46 > v35)
        //                                    {
        //                                        v47 = (v38 - 1) | v35;
        //                                        v48 = v47 < 0;
        //                                        if (v47 >= 0)
        //                                            v48 = *(v36 + 100) < v38;
        //                                        if (!v48)
        //                                        {
        //                                            v32 = *(*v36 + 4 * (v35 + v46 * (v38 - 1)));
        //                                            if (v32)
        //                                            {
        //                                                if (a6)
        //                                                    ShouldDestruct(*v32);
        //                                                else
        //                                                    sub_2DD460(v32);
        //                                                v36 = a1;
        //                                                v35 = v62;
        //                                            }
        //                                        }
        //                                    }
        //                                    v49 = *(v36 + 96);
        //                                    if (v49 <= v35)
        //                                        goto LABEL_76;
        //                                    v50 = v38 + 1;
        //                                    v51 = (v38 + 1) | v35;
        //                                    v52 = v51 <= 0;
        //                                    if (v51 >= 0)
        //                                        v52 = *(v36 + 100) <= v50;
        //                                    if (v52 || (v33 = *(*v36 + 4 * (v35 + v49 * v50))) == 0)
        //                                    {
        //                                    LABEL_76:
        //                                        v24 = v57;
        //                                        v25 = a7;
        //                                    }
        //                                    else
        //                                    {
        //                                        if (a6)
        //                                            ShouldDestruct(*v33);
        //                                        else
        //                                            sub_2DD460(v33);
        //                                        v24 = v57;
        //                                        v25 = a7;
        //                                    }
        //                                    if ((sub_3DA7C8)(a5, a2, a3, a4, a5))
        //                                        return 0;
        //                                    LABEL_78:
        //                                    v23 = v62;
        //                                    v28 += 300;
        //                                    v26 = a1;
        //                                    v21 = v38 + 1;
        //                                    if (v38 >= v24)
        //                                    {
        //                                        result = 1;
        //                                        v20 = v54;
        //                                        v21 = v53;
        //                                        v27 = v61;
        //                                        goto LABEL_80;
        //                                    }
        //                                }
        //                                v41 = sub_7A334C(*v39);
        //                                goto LABEL_54;
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return result;
        //}

        public Tile GetTile(int x, int y, bool isTile = false, bool OnlyRealTiles = false)
        {
            if (!isTile)
            {
                x = LogicToTile(x);
                y = LogicToTile(y);
            }
            if (!OnlyRealTiles)
            {
                foreach (Tile tile in NewTiles)
                {
                    if (tile.TileX == x && tile.TileY == y && !tile.IsDestructed()) return tile;
                }
            }
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                return Tiles[y, x];
            }


            return null;
        }

        public static int LogicToTile(int logicValue)
        {
            return logicValue / 300;
        }

        public static int TileToLogic(int tile)
        {
            return 300 * tile;
        }

        internal Tile[,] GetTiles()
        {
            return Tiles;
        }
        public void DestroyEnvironment(
        int a2,
        int a3,
        int a4,
        bool a5)
        {
            int v7; // r10
            int v8; // r7
            int v9; // r6
            int v10; // r9
            int v11; // r8 MAPDST
            int v12; // r5
            int v13; // r7
            int v14; // r8
            int v15; // r0
            int v16; // r9
            Tile v17; // r10
            bool v18; // r0
            //int v20; // r8
            //_DWORD* v21; // r0
            //int v22; // r9
            //int v23; // r6
            //unsigned int v24; // r7
            int v26; // [sp+8h] [bp-40h]
            int v27; // [sp+8h] [bp-40h]
            int v30; // [sp+14h] [bp-34h]
            //int v32; // [sp+1Ch] [bp-2Ch] BYREF
            //void* p[2]; // [sp+20h] [bp-28h] BYREF
            int v34; // [sp+28h] [bp-20h]
            v7 = a2 / 300;
            v8 = a4 / 300 + 1;
            v9 = LogicMath.Clamp(a2 / 300 - v8, 0, LogicWidth - 1);
            v26 = a3;
            v10 = a3 / 300;
            v11 = LogicMath.Clamp(a3 / 300 - v8, 0, LogicHeight - 1);
            v12 = LogicMath.Clamp(v8 + v7, 0, LogicWidth - 1);
            v13 = LogicMath.Clamp(v8 + v10, 0, LogicHeight - 1);
            v34 = 0;
            //p[1] = 0;
            //p[0] = 0;
            if (v9 <= v12)
            {
                v30 = a4 * a4;
                v27 = 300 * v11 + 150 - v26;
                do
                {
                    if (v11 <= v13)
                    {
                        v14 = v27;
                        v15 = v11;
                        do
                        {
                            v16 = v15;
                            //v32 = v9 + *(a1 + 96) * v15;
                            //v17 = *(*a1 + 4 * v32);
                            v17 = GetTile(v9, v15, true);
                            if (v17 == null) goto LABEL_1;
                            if (a5)
                                v18 = v17.Data.IsDestructibleNormalWeapon;
                            else
                                v18 = v17.Data.IsDestructible;
                            if (v18 && (150 - a2 + 300 * v9) * (150 - a2 + 300 * v9) + v14 * v14 <= v30)
                            {
                                //if (sub_5B00F0(*v17))
                                //{
                                //    List::Add(p, &v32);
                                //    *(a1 + 132) = 1;
                                //}
                                v17.Destruct();
                            }
                        LABEL_1:
                            v14 += 300;
                            v15 = v16 + 1;
                        }
                        while (v16 < v13);
                    }
                }
                while (v9++ < v12);
                //v20 = v34;
                //v21 = p[0];
                //if (v34 >= 1)
                //{
                //    v22 = 0;
                //    do
                //    {
                //        v23 = v21[v22];
                //        v24 = 1;
                //        do
                //        {
                //            if (!sub_583074(a1, v23, v24 - 1, a5))
                //                break;
                //        }
                //        while (v24++ < 0xA);
                //        v21 = p[0];
                //        ++v22;
                //    }
                //    while (v22 != v20);
                //}
                //if (v21)
                //    j_j_j_free_0(v21);
            }
        }
    }
}
