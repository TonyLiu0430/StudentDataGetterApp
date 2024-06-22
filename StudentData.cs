using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StudentDataGetterApp {
    public class Department {
        public readonly static List<Department> 日間部學士班 = new List<Department>{
            //new Department("71", "法律學系"), //###
            new Department("712", "法律學系法學組"),
            new Department("714", "法律學系司法組"),
            new Department("716", "法律學系財經法組"),
            new Department("72", "公共行政暨政策學系"),
            new Department("73", "經濟學系"),
            new Department("742", "社會學系"),
            new Department("744", "社會工作學系"),
            new Department("75", "財政學系"),
            new Department("76", "不動產與城鄉環境學系"),
            new Department("77", "會計學系"),
            new Department("78", "統計學系"),
            new Department("79", "企業管理學系"),
            new Department("80", "金融與合作經營學系"),
            new Department("81", "中國文學系"),
            new Department("82", "應用外語學系"),
            new Department("83", "歷史學系"),
            new Department("84", "休閒運動管理學系"),
            new Department("85", "資訊工程學系"),
            new Department("86", "通訊工程學系"),
            new Department("87", "電機工程學系"),
        };
        public string Name { get; set; }
        public string Id { get; set; }

        public Department(string id, string name) {
            this.Id = id;
            this.Name = name;
        }

        public static Department GetDepartmentFromStudent(string studentId) { //s411285029
            List<Department> departmentList = null;
            if (int.Parse(studentId[1].ToString()) == 4) {
                departmentList = 日間部學士班;
            }
            /*......TODO 其他部別  ......*/

            if (departmentList == null) {
                MessageBox.Show("暫不支援部別", "錯誤", MessageBoxButton.OK, MessageBoxImage.Error);
                throw new Exception("暫不支援部別");
            }

            string studentDepartmentId = studentId.Substring(5, 3);
            foreach (var department in departmentList) {
                if (studentDepartmentId.StartsWith(department.Id)) {
                    return department;
                }
            }
            return null;
        }
    }

    public class Student{
        public string Name { get; set; }
        public string Id { get; set; }
        public Department Department { get; set; }
    }

    public class StudentComparer : IComparer<Student> {
        public int Compare(Student x, Student y) {
            return x.Id.CompareTo(y.Id);
        }
    }   
}
