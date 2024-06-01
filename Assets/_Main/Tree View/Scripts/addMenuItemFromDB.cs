using Mono.Data.Sqlite;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class addMenuItemFromDB : MonoBehaviour
{
    [SerializeField]
    private GameObject menuItemPrefab; // Prefab cho mục menu

    [SerializeField]
    private Transform menuParent; // Đối tượng cha chứa các mục menu

    private string connectionString; // Chuỗi kết nối đến cơ sở dữ liệu SQLite

    private void Start()
    {
        // Thiết lập chuỗi kết nối đến cơ sở dữ liệu (thay đổi tùy theo vị trí và tên cơ sở dữ liệu của bạn)
        connectionString = "URI=file:E:/project/Disassembly_and_Assembly_weapon/Assets/db.db";


        // Gọi phương thức để thêm mục từ cơ sở dữ liệu
        AddMenuItemsFromDatabase();
    }

    private void AddMenuItemsFromDatabase()
    {
        // Kết nối đến cơ sở dữ liệu
        IDbConnection dbConnection = new SqliteConnection(connectionString);
        dbConnection.Open();

        // Lấy dữ liệu từ cơ sở dữ liệu
        IDbCommand dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = "SELECT name FROM weapon_groups WHERE weapon_groups_id =1";
        IDataReader reader = dbCommand.ExecuteReader();

        // Duyệt qua các dòng dữ liệu và thêm mục vào menu
        while (reader.Read())
        {
            string itemName = reader.GetString(0);

            // Tạo một đối tượng mới từ prefab và đặt tên cho nó
            GameObject newItem = Instantiate(menuItemPrefab, menuParent);
            newItem.name = "MenuItem_" + itemName;

            // Cập nhật nội dung cho mục mới
            Text itemText = newItem.GetComponentInChildren<Text>();
            if (itemText != null)
            {
                itemText.text = itemName;
            }
        }

        // Đóng kết nối sau khi sử dụng
        reader.Close();
        reader = null;
        dbCommand.Dispose();
        dbCommand = null;
        dbConnection.Close();
    }
}
