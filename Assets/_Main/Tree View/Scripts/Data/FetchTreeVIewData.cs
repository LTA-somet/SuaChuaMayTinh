
using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

namespace Nam.TreeView
{
    public class FetchTreeVIewData : MonoBehaviour
    {
        private void Start()
        {
            //Debug.Log(GetTreeViewGroupWeaponDatas()[0].ID);
            //Debug.Log(GetTreeViewWeaponDatas("AK")[0].ID);
        }
        private List<TreeViewData> GetTreeViewData(string tableName, string condition)
        {
            List<TreeViewData> treeData = new List<TreeViewData>();

            using (IDbConnection dbconn = new SqliteConnection("URI=file:" /*+ UIManager.Instance.DBPath*/))
            {
                dbconn.Open();

                // Tạo câu truy vấn cho dữ liệu từ bảng tableName với điều kiện condition, sắp xếp theo sequence ASC
                string query = $"SELECT * FROM {tableName} {condition} ORDER BY sequence ASC";
                // Tạo câu truy vấn cho dữ liệu từ bảng data với mẫu data_id là dataIdPattern và ID của vũ khí hoặc nhóm vũ khí
                using (IDbCommand weaponDbCommand = dbconn.CreateCommand())
                {
                    weaponDbCommand.CommandText = query;
                    using (IDataReader weaponReader = weaponDbCommand.ExecuteReader())
                    {
                        while (weaponReader.Read())
                        {
                            TreeViewData newSlide = new TreeViewData()
                            {
                                ID = !weaponReader.IsDBNull(0) ? weaponReader.GetInt16(0) : 1,
                                Name = !weaponReader.IsDBNull(1) ? weaponReader.GetString(1) : string.Empty,
                                Index = !weaponReader.IsDBNull(2) ? weaponReader.GetInt16(2) : 1,
                                Datapath = !weaponReader.IsDBNull(3) ? weaponReader.GetString(3) : string.Empty,
                            };
                            treeData.Add(newSlide);
                        }
                    }
                }
            }

            return treeData;
        }

        // Phương thức này trả về danh sách TreeViewData cho vũ khí dựa trên id của nhóm vũ khí.
        public List<TreeViewData> GetTreeViewWeaponDatas(int id)
        {
            string tableName = "weapon";
            string condition = $"WHERE weapon_groups_id = {id}";

            return GetTreeViewData(tableName, condition);
        }

        // Phương thức này trả về danh sách TreeViewData cho nhóm vũ khí.
        public List<TreeViewData> GetTreeViewGroupWeaponDatas()
        {
            string tableName = "weapon_groups";
            string condition = ""; // Bạn cần chỉ định điều kiện tương ứng ở đây

            return GetTreeViewData(tableName, condition);
        }

    }
}

