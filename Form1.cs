using MySql.Data.MySqlClient;
using System.Data;

namespace Driver_s_Course_Terminal
{
    public partial class Form1 : Form
    {
        // Create lists
        CourseAnalysis courseAnalysis = new CourseAnalysis();

        public Form1()
        {
            InitializeComponent();
            labelApplicationName.Text = Application.CompanyName;
            loadCourseAnalysis();
        }

        // Course analysis load
        private void loadCourseAnalysis()
        {
            // Initialize database connection and data adapter
            DB dB = new DB();
            DataTable dt = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlConnection conn = dB.GetConnection();


            // Inizialize courses
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM `course`", conn);
            adapter.SelectCommand = cmd;
            adapter.Fill(dt);

            foreach (DataRow row in dt.Rows)
            {
                int idCourse = Convert.ToInt32(row["idCourse"]);
                string name = row["Name"].ToString();
                double cost = Convert.ToDouble(row["Cost"]);
                int durationInDays = Convert.ToInt32(row["DurationInDays"]);
                Course course = new Course(idCourse, name, cost, durationInDays);

                // Inizialize course has students
                cmd = new MySqlCommand("SELECT * FROM `course_has_students` WHERE `Course_idCourse` = @cId", conn);
                cmd.Parameters.Add("@cId", MySqlDbType.VarChar).Value = idCourse;
                adapter.SelectCommand = cmd;

                DataTable course_has_students_dt = new DataTable();
                adapter.Fill(course_has_students_dt);


                foreach (DataRow c_has_s_Row in course_has_students_dt.Rows)
                {
                    // Inizialize student
                    cmd = new MySqlCommand("SELECT * FROM `students` WHERE `idStudents` = @sId", conn);
                    cmd.Parameters.Add("@sId", MySqlDbType.VarChar).Value = c_has_s_Row["Students_idStudents"];
                    adapter.SelectCommand = cmd;

                    DataTable studentdt = new DataTable();
                    adapter.Fill(studentdt);

                    foreach (DataRow studentrow in studentdt.Rows)
                    {
                        int sStudentsId = Convert.ToInt32(studentrow["idStudents"]);
                        string sName = studentrow["Name"].ToString();
                        string sCategory = studentrow["Category"].ToString();
                        Student student = new Student(sStudentsId, sName, sCategory);


                        // Create temp datatable, adapter and command
                        DataTable tdataTable = new DataTable();
                        MySqlDataAdapter tadapter;
                        MySqlCommand tcmd;

                        // Inizialize attedance
                        tcmd = new MySqlCommand("SELECT * FROM `attendance` WHERE `Students_idStudents` = @sId", conn);
                        tcmd.Parameters.Add("@sId", MySqlDbType.VarChar).Value = sStudentsId;
                        tadapter = new MySqlDataAdapter(tcmd);
                        tadapter.Fill(tdataTable);

                        foreach (DataRow trow in tdataTable.Rows)
                        {
                            int aAttendanceId = Convert.ToInt32(trow["idAttendance"]);
                            int aStudentsId = Convert.ToInt32(trow["Students_idStudents"]);
                            int aCourseId = Convert.ToInt32(trow["Course_idCourse"]);
                            DateTime aDate = Convert.ToDateTime(trow["Date"]);
                            bool aIsPresent = Convert.ToInt32(trow["IsPresent"]) == 0 ? false : true;
                            Attendance attendance = new Attendance(aAttendanceId, aStudentsId, aCourseId, aDate, aIsPresent);
                            student.Attendances.Add(attendance);
                        }


                        // Inizialize driving practice
                        tcmd = new MySqlCommand("SELECT * FROM `drivingpractice` WHERE `Students_idStudents` = @sId", conn);
                        tcmd.Parameters.Add("@sId", MySqlDbType.VarChar).Value = sStudentsId;
                        tadapter = new MySqlDataAdapter(tcmd);
                        tdataTable.Reset();
                        tadapter.Fill(tdataTable);

                        foreach (DataRow trow in tdataTable.Rows)
                        {
                            int pPracticeId = Convert.ToInt32(trow["idDrivingPractice"]);
                            int pStudentsId = Convert.ToInt32(trow["Students_idStudents"]);
                            DateTime pDate = Convert.ToDateTime(trow["Date"]);
                            string pInstructor = trow["Instructor"].ToString();
                            double pDurationInHours = Convert.ToDouble(trow["DurationInHours"]);

                            DrivingPractice practice = new DrivingPractice(pPracticeId, pStudentsId, pDate, pInstructor, pDurationInHours);
                            student.DrivingPractices.Add(practice);
                        }


                        // Inizialize driving errors
                        tcmd = new MySqlCommand("SELECT * FROM `drivingerrors` WHERE `Students_idStudents` = @sId", conn);
                        tcmd.Parameters.Add("@sId", MySqlDbType.VarChar).Value = sStudentsId;
                        tadapter = new MySqlDataAdapter(tcmd);
                        tdataTable.Reset();
                        tadapter.Fill(tdataTable);

                        foreach (DataRow trow in tdataTable.Rows)
                        {
                            int eErrorsId = Convert.ToInt32(trow["idDrivingErrors"]);
                            int eStudentsId = Convert.ToInt32(trow["Students_idStudents"]);
                            DateTime eDate = Convert.ToDateTime(trow["Date"]);
                            string eDescription = trow["ErrorDescription"].ToString();

                            DrivingErrors error = new DrivingErrors(eErrorsId, eStudentsId, eDate, eDescription);
                            student.DrivingErrors.Add(error);
                        }


                        // Inizialize test results
                        tcmd = new MySqlCommand("SELECT * FROM `testresults` WHERE `Students_idStudents` = @sId", conn);
                        tcmd.Parameters.Add("@sId", MySqlDbType.VarChar).Value = sStudentsId;
                        tadapter = new MySqlDataAdapter(tcmd);
                        tdataTable.Reset();
                        tadapter.Fill(tdataTable);

                        foreach (DataRow trow in tdataTable.Rows)
                        {
                            int tsTestId = Convert.ToInt32(trow["idTestResults"]);
                            int tsStudentId = Convert.ToInt32(trow["Students_idStudents"]);
                            DateTime tsDate = Convert.ToDateTime(trow["Date"]);
                            double tsScore = Convert.ToDouble(trow["Score"]);

                            TestResults test = new TestResults(tsTestId, tsStudentId, tsDate, tsScore);
                            student.TestResults.Add(test);
                        }


                        // Inizialize exam results
                        tcmd = new MySqlCommand("SELECT * FROM `examresults` WHERE `Students_idStudents` = @sId", conn);
                        tcmd.Parameters.Add("@sId", MySqlDbType.VarChar).Value = sStudentsId;
                        tadapter = new MySqlDataAdapter(tcmd);
                        tdataTable.Reset();
                        tadapter.Fill(tdataTable);

                        foreach (DataRow trow in tdataTable.Rows)
                        {
                            int exExamId = Convert.ToInt32(trow["idExamResults"]);
                            int exStudentId = Convert.ToInt32(trow["Students_idStudents"]);
                            DateTime exDate = Convert.ToDateTime(trow["Date"]);
                            bool exIsPassed = Convert.ToInt32(trow["IsPassed"]) == 0 ? false : true;

                            ExamResults exam = new ExamResults(exExamId, exStudentId, exDate, exIsPassed);
                            student.ExamResults.Add(exam);
                        }

                        course.Students.Add(student);
                    }
                }
                courseAnalysis.AddCourse(course);
            }
        }



        // Application
        private void labelCloseApp_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void labelCloseApp_MouseEnter(object sender, EventArgs e)
        {
            labelCloseApp.BackColor = Color.IndianRed;
            labelCloseApp.ForeColor = SystemColors.Menu;
        }

        private void labelCloseApp_MouseLeave(object sender, EventArgs e)
        {
            labelCloseApp.BackColor = SystemColors.Menu;
            labelCloseApp.ForeColor = Color.IndianRed;
        }

        Point lastPoint;
        private void panelMenuApplication_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Left += e.X - lastPoint.X;
                this.Top += e.Y - lastPoint.Y;
            }
        }

        private void panelMenuApplication_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = new Point(e.X, e.Y);
        }



        // Students
        private void tabPageStudents_Enter(object sender, EventArgs e)
        {
            textBoxStudentsFilter_TextChanged(sender, e);
        }

        private void buttonStudentsAddList_Click(object sender, EventArgs e)
        {
            string name = textBoxStudentsStudentName.Text;
            string category = comboBoxStudentsCategory.Text;
            // Checking for an empty string
            if (textBoxStudentsStudentName.Text.Length < 1)
            {
                comboBoxStudentsCategory.SelectedIndex = 0;
                MessageBox.Show("Пожалуйста введите имя!");
                return;
            }

            DB dB = new DB();

            MySqlCommand cmd = new MySqlCommand("INSERT INTO `students` (`Name`, `Category`) VALUES (@sN, @sct);", dB.GetConnection());
            cmd.Parameters.Add("@sN", MySqlDbType.VarChar).Value = name;
            cmd.Parameters.Add("@sct", MySqlDbType.VarChar).Value = category;

            dB.openConnection();

            if (cmd.ExecuteNonQuery() == 1)
                MessageBox.Show("Студент успешно добавлен!");
            else
                MessageBox.Show("Ошибка при регистрации!");

            textBoxStudentsStudentName.Text = "";
            comboBoxStudentsCategory.SelectedIndex = 0;

            dB.closeConnection();

            tabPageStudents_Enter(sender, e);
        }

        private void textBoxStudentsFilter_TextChanged(object sender, EventArgs e)
        {
            // Initialize database connection and data adapter
            DB dB = new DB();
            DataTable dt = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            string filter = textBoxStudentsFilter.Text;

            // Create and configure the SQL command
            MySqlCommand cmd = new MySqlCommand("SELECT `idStudents`, `Name`, `Category` FROM `students`\r\n" +
                "WHERE `Name` LIKE @fN;", dB.GetConnection());

            cmd.Parameters.Add("@fN", MySqlDbType.VarChar).Value = filter + "%";

            // Assign the command to the adapter
            adapter.SelectCommand = cmd;

            // Fill the DataTable with data from the database
            adapter.Fill(dt);

            // Clear the DataGridView before adding new items
            dataGridViewStudents.DataSource = null;
            dataGridViewStudents.Columns.Clear();
            comboBoxStudentsCategory.SelectedIndex = 0;

            // Set the DataSource of the DataGridView to the DataTable
            dataGridViewStudents.DataSource = dt;

            // Adjust column headers if necessary
            dataGridViewStudents.Columns["idStudents"].HeaderText = "Id";
            dataGridViewStudents.Columns["Name"].HeaderText = "ФИО студента";
            dataGridViewStudents.Columns["Category"].HeaderText = "Категория";

            dataGridViewStudents.Columns["idStudents"].Width = 70;
            dataGridViewStudents.Columns["Name"].Width = 170;
            dataGridViewStudents.Columns["Category"].Width = 110;
        }

        int idStudents;
        private void buttonStudentsEditing_Click(object sender, EventArgs e)
        {
            buttonStudentsAddList.Visible = false;
            buttonStudentsEdit.Visible = true;
            buttonStudentsCancel.Visible = true;

            idStudents = int.Parse(dataGridViewStudents[0, dataGridViewStudents.SelectedCells[0].RowIndex].Value.ToString());
            textBoxStudentsStudentName.Text = dataGridViewStudents[1, dataGridViewStudents.SelectedCells[0].RowIndex].Value.ToString();
            comboBoxStudentsCategory.SelectedItem = dataGridViewStudents[2, dataGridViewStudents.SelectedCells[0].RowIndex].Value;
        }
        private void buttonStudentsEdit_Click(object sender, EventArgs e)
        {
            // Checking for an empty string
            if (textBoxStudentsStudentName.Text.Length < 1 ||
                comboBoxStudentsCategory.Text.Length < 1)
            {
                MessageBox.Show("Пожалуйста заполенте все поля!");
                return;
            }

            DB dB = new DB();

            MySqlCommand cmd = new MySqlCommand("UPDATE `students` SET `Name` = @sN, `Category` = @ct WHERE `students`.`idStudents` = @sId", dB.GetConnection());
            cmd.Parameters.Add("@sId", MySqlDbType.VarChar).Value = idStudents;
            cmd.Parameters.Add("@sN", MySqlDbType.VarChar).Value = textBoxStudentsStudentName.Text;
            cmd.Parameters.Add("@ct", MySqlDbType.VarChar).Value = comboBoxStudentsCategory.Text;


            dB.openConnection();

            if (cmd.ExecuteNonQuery() == 1)
                MessageBox.Show("Запись изменена!");
            else
                MessageBox.Show("Ошибка при редактировании!");

            textBoxStudentsStudentName.Text = "";
            comboBoxStudentsCategory.SelectedIndex = 0;

            // Restore buttons visible
            buttonStudentsCancel.Visible = false;
            buttonStudentsEdit.Visible = false;
            buttonStudentsAddList.Visible = true;

            dB.closeConnection();

            tabPageStudents_Enter(sender, e);
        }
        private void buttonStudentsCancel_Click(object sender, EventArgs e)
        {
            textBoxStudentsStudentName.Text = "";
            comboBoxStudentsCategory.SelectedIndex = 0;

            // Restore buttons visible
            buttonStudentsCancel.Visible = false;
            buttonStudentsEdit.Visible = false;
            buttonStudentsAddList.Visible = true;
        }



        // Courses
        private void tabPageCourses_Enter(object sender, EventArgs e)
        {
            textBoxCourseFilter_TextChanged(sender, e);
        }

        private void buttonCourseAddList_Click(object sender, EventArgs e)
        {
            // Checking for an empty string
            if (textBoxCourseName.Text.Length < 1)
            {
                MessageBox.Show("Пожалуйста введите имя!");
                return;
            }

            string name = textBoxCourseName.Text;
            double cost = (double)numericUpDownCourseCost.Value;
            int durationInDays = (int)numericUpDownCourseDurationInDays.Value;

            DB dB = new DB();

            MySqlCommand cmd = new MySqlCommand("INSERT INTO `course` (`Name`, `Cost`, `DurationInDays`) VALUES (@cN, @cC, @cD);", dB.GetConnection());
            cmd.Parameters.Add("@cN", MySqlDbType.VarChar).Value = name;
            cmd.Parameters.Add("@cC", MySqlDbType.VarChar).Value = cost;
            cmd.Parameters.Add("@cD", MySqlDbType.VarChar).Value = durationInDays;


            dB.openConnection();

            if (cmd.ExecuteNonQuery() == 1)
            {
                DataTable dt = new DataTable();
                cmd = new MySqlCommand("SELECT * FROM `course` WHERE `idCourse` = (SELECT MAX(`idCourse`) FROM `course`);", dB.GetConnection());
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    int courseId = Convert.ToInt32(row["idCourse"]);
                    Course course = new Course(courseId, name, cost, durationInDays);
                    courseAnalysis.AddCourse(course);
                }
                MessageBox.Show("Курс успешно добавлен!");
            }
            else
                MessageBox.Show("Ошибка при добавлении!");

            textBoxCourseName.Text = "";
            numericUpDownCourseCost.Value = 1;
            numericUpDownCourseDurationInDays.Value = 1;

            dB.closeConnection();

            textBoxCourseFilter_TextChanged(sender, e);
        }

        private void textBoxCourseFilter_TextChanged(object sender, EventArgs e)
        {
            // Initialize database connection and data adapter
            DB dB = new DB();
            DataTable dt = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            string filter = textBoxCourseFilter.Text;

            // Create and configure the SQL command
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM `course`\r\n" +
                "WHERE `Name` LIKE @fN;", dB.GetConnection());

            cmd.Parameters.Add("@fN", MySqlDbType.VarChar).Value = filter + "%";

            // Assign the command to the adapter
            adapter.SelectCommand = cmd;

            // Fill the DataTable with data from the database
            adapter.Fill(dt);

            // Clear the DataGridView before adding new items
            dataGridViewCourse.DataSource = null;
            dataGridViewCourse.Columns.Clear();

            // Set the DataSource of the DataGridView to the DataTable
            dataGridViewCourse.DataSource = dt;

            // Adjust column headers if necessary
            dataGridViewCourse.Columns["idCourse"].HeaderText = "Id";
            dataGridViewCourse.Columns["Name"].HeaderText = "Название курса";
            dataGridViewCourse.Columns["Cost"].HeaderText = "Стоимость";
            dataGridViewCourse.Columns["DurationInDays"].HeaderText = "Длительность\n(дн.)";

            dataGridViewCourse.Columns["idCourse"].Width = 70;
            dataGridViewCourse.Columns["Name"].Width = 330;
            dataGridViewCourse.Columns["Cost"].Width = 120;
            dataGridViewCourse.Columns["DurationInDays"].Width = 140;
        }



        // Course has students
        private void tabPageCourseHasStudents_Enter(object sender, EventArgs e)
        {
            // Refresh dataTable
            textBoxCourseHasStudents_TextChanged(sender, e);

            // Initialize database connection and data adapter
            DB dB = new DB();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            // DataTable for the combobox
            DataTable dtStudensNames = new DataTable();
            DataTable dtCoursesNames = new DataTable();

            MySqlCommand cmdStudensNames = new MySqlCommand("SELECT `idStudents`, `Name` FROM `students`", dB.GetConnection());
            MySqlCommand cmdCoursesNames = new MySqlCommand("SELECT `idCourse`, `Name` FROM `course`", dB.GetConnection());

            dB.openConnection();

            adapter.SelectCommand = cmdCoursesNames;
            adapter.Fill(dtCoursesNames);
            comboBoxCourseHasStudentsCourseName.DataSource = dtCoursesNames;
            comboBoxCourseHasStudentsCourseName.DisplayMember = "Name";
            comboBoxCourseHasStudentsCourseName.ValueMember = "idCourse";


            adapter.SelectCommand = cmdStudensNames;
            adapter.Fill(dtStudensNames);
            comboBoxCourseHasStudentsStudentName.DataSource = dtStudensNames;
            comboBoxCourseHasStudentsStudentName.DisplayMember = "Name";
            comboBoxCourseHasStudentsStudentName.ValueMember = "idStudents";

            dB.closeConnection();
        }

        private void buttonCourseHasStudents_Click(object sender, EventArgs e)
        {
            // Checking for an empty string
            if (comboBoxCourseHasStudentsStudentName.Text.Length < 1 ||
                comboBoxCourseHasStudentsCourseName.Text.Length < 1)
            {
                MessageBox.Show("Пожалуйста заполенте все поля!");
                return;
            }

            DB dB = new DB();

            int studentId = Convert.ToInt32(comboBoxCourseHasStudentsStudentName.SelectedValue);
            int courseId = Convert.ToInt32(comboBoxCourseHasStudentsCourseName.SelectedValue);

            MySqlCommand cmd = new MySqlCommand("INSERT INTO `course_has_students` (`Students_idStudents`, `Course_idCourse`)VALUES (@sId, @cId);", dB.GetConnection());
            cmd.Parameters.Add("@sId", MySqlDbType.VarChar).Value = studentId;
            cmd.Parameters.Add("@cId", MySqlDbType.VarChar).Value = courseId;

            dB.openConnection();

            if (cmd.ExecuteNonQuery() == 1)
            {
                cmd = new MySqlCommand("SELECT * FROM `students` WHERE `idStudents` = @sId;", dB.GetConnection());
                cmd.Parameters.Add("@sId", MySqlDbType.VarChar).Value = studentId;
                DataTable dt = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    string name = Convert.ToString(row["Name"]);
                    string category = Convert.ToString(row["Category"]);

                    Student student = new Student(studentId, name, category);
                    courseAnalysis.AddCoursehasStudent(courseId, student);
                }
                MessageBox.Show("Запись добавлена!");
            }
            else
                MessageBox.Show("Ошибка при добавлении!");

            comboBoxCourseHasStudentsStudentName.SelectedIndex = 0;
            comboBoxCourseHasStudentsCourseName.SelectedIndex = 0;

            dB.closeConnection();

            tabPageCourseHasStudents_Enter(sender, e);
        }

        private void textBoxCourseHasStudents_TextChanged(object sender, EventArgs e)
        {
            // Initialize database connection and data adapter
            DB dB = new DB();
            DataTable dt = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            string filter = textBoxCourseHasStudentsFilter.Text;

            // Create and configure the SQL command
            MySqlCommand cmd = new MySqlCommand("SELECT `idCourse_has_Students`, `students`.`Name` AS `studentName`, `course`.`Name` AS `courseName` FROM `course_has_students`\r\n" +
                "LEFT JOIN `students` ON `Students_idStudents` = `students`.`idStudents`\r\n" +
                "LEFT JOIN `course` on `Course_idCourse` = `course`.`idCourse`\r\n" +
                "WHERE `students`.`Name` LIKE @fN\r\n" +
                "ORDER BY `idCourse_has_Students`;", dB.GetConnection());

            cmd.Parameters.Add("@fN", MySqlDbType.VarChar).Value = filter + "%";

            // Assign the command to the adapter
            adapter.SelectCommand = cmd;

            // Fill the DataTable with data from the database
            adapter.Fill(dt);

            // Clear the DataGridView before adding new items
            dataGridViewCourseHasStudents.DataSource = null;
            dataGridViewCourseHasStudents.Columns.Clear();

            // Set the DataSource of the DataGridView to the DataTable
            dataGridViewCourseHasStudents.DataSource = dt;

            // Adjust column headers if necessary
            dataGridViewCourseHasStudents.Columns["idCourse_has_Students"].HeaderText = "Id";
            dataGridViewCourseHasStudents.Columns["studentName"].HeaderText = "ФИО студента";
            dataGridViewCourseHasStudents.Columns["courseName"].HeaderText = "Название курса";

            dataGridViewCourseHasStudents.Columns["idCourse_has_Students"].Width = 70;
            dataGridViewCourseHasStudents.Columns["studentName"].Width = 170;
            dataGridViewCourseHasStudents.Columns["courseName"].Width = 330;
        }



        // Attendances
        private void tabPageAttendance_Enter(object sender, EventArgs e)
        {
            // Refresh dataTable
            textBoxStudentAttedanceFilter_TextChanged(sender, e);
            
            // Initialize database connection and data adapter
            DB dB = new DB();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            DataTable dtStudensNames = new DataTable();

            MySqlCommand cmdStudensNames = new MySqlCommand("SELECT `idStudents`, `Name` FROM `students`", dB.GetConnection());

            dB.openConnection();
            adapter.SelectCommand = cmdStudensNames;
            adapter.Fill(dtStudensNames);

            comboBoxAttendanceStudentName.DataSource = dtStudensNames;
            comboBoxAttendanceStudentName.DisplayMember = "Name";
            comboBoxAttendanceStudentName.ValueMember = "idStudents";

            LoadCoursesForStudent(Convert.ToInt32(comboBoxAttendanceStudentName.SelectedValue));

            dB.closeConnection();
        }

        private void LoadCoursesForStudent(int studentId)
        {
            // Initialize database connection and data adapter
            DB dB = new DB();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            DataTable dtCoursesNames = new DataTable();

            // Get courses for the selected student
            MySqlCommand cmd_course_has_students = new MySqlCommand(
                "SELECT `course`.`idCourse`, `course`.`Name` FROM `course` " +
                "INNER JOIN `course_has_students` ON `course`.`idCourse` = `course_has_students`.`Course_idCourse` " +
                "WHERE `course_has_students`.`Students_idStudents` = @studentId", dB.GetConnection());
            cmd_course_has_students.Parameters.Add("@studentId", MySqlDbType.Int32).Value = studentId;

            dB.openConnection();
            adapter.SelectCommand = cmd_course_has_students;
            adapter.Fill(dtCoursesNames);
            dB.closeConnection();

            // Set the DataSource of the ComboBox to the DataTable
            comboBoxAttendanceCourseName.DataSource = dtCoursesNames;
            comboBoxAttendanceCourseName.DisplayMember = "Name";
            comboBoxAttendanceCourseName.ValueMember = "idCourse";
        }

        private void comboBoxAttendanceStudentName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxAttendanceStudentName.SelectedValue is int studentId)
            {
                LoadCoursesForStudent(studentId);
            }
        }

        private void buttonAttendanceAddList_Click(object sender, EventArgs e)
        {
            // Checking for an empty string
            if (comboBoxAttendanceStudentName.Text.Length < 1 ||
                comboBoxAttendanceCourseName.Text.Length < 1 ||
                dateTimePickerAttendanceDateTime.Text.Length < 1)
            {
                MessageBox.Show("Пожалуйста заполенте все поля!");
                return;
            }

            DB dB = new DB();

            int studentId = Convert.ToInt32(comboBoxAttendanceStudentName.SelectedValue);
            int courseId = Convert.ToInt32(comboBoxAttendanceCourseName.SelectedValue);
            DateTime date = Convert.ToDateTime(dateTimePickerAttendanceDateTime.Value);
            bool isPresent = checkBoxAttendaceIsPresent.Checked;

            MySqlCommand cmd = new MySqlCommand("INSERT INTO `attendance` (`Date`, `IsPresent`, `Students_idStudents`, `Course_idCourse`) VALUES (@tD, @iP, @sId, @cId);", dB.GetConnection());
            cmd.Parameters.AddWithValue("@tD", date);
            cmd.Parameters.Add("@iP", MySqlDbType.VarChar).Value = isPresent == true ? 1 : 0;
            cmd.Parameters.Add("@sId", MySqlDbType.VarChar).Value = studentId;
            cmd.Parameters.Add("@cId", MySqlDbType.VarChar).Value = courseId;

            dB.openConnection();

            if (cmd.ExecuteNonQuery() == 1)
            {
                DataTable dt = new DataTable();
                cmd = new MySqlCommand("SELECT * FROM `attendance` WHERE `idAttendance` = (SELECT MAX(`idAttendance`) FROM `attendance`);", dB.GetConnection());
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    int attendanceId = Convert.ToInt32(row["idAttendance"]);
                    Attendance attendance = new Attendance(attendanceId, studentId, courseId, date, isPresent);
                    courseAnalysis.AddAttendance(courseId, studentId, attendance);
                }
                MessageBox.Show("Запись добавлена!");
            }
            else
                MessageBox.Show("Ошибка при добавлении!");

            comboBoxAttendanceStudentName.SelectedIndex = 0;

            dB.closeConnection();

            tabPageAttendance_Enter(sender, e);
        }

        private void textBoxStudentAttedanceFilter_TextChanged(object sender, EventArgs e)
        {
            // Initialize database connection and data adapter
            DB dB = new DB();
            DataTable dt = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            string filter = textBoxStudentAttedanceFilter.Text;

            // Create and configure the SQL command
            MySqlCommand cmd = new MySqlCommand("SELECT `attendance`.`idAttendance`, `attendance`.`Date`, `attendance`.`IsPresent`, `students`.`Name` AS `studentsName`, `course`.`Name` AS `courseName` FROM `students`\r\n" +
                "INNER JOIN `attendance` ON `attendance`.`Students_idStudents` = `students`.`idStudents`\r\n" +
                "LEFT JOIN `course` ON `attendance`.`Course_idCourse` = `course`.`idCourse`\r\n" +
                "WHERE `students`.`Name` LIKE @fN\r\n" +
                "ORDER BY `attendance`.`idAttendance`;", dB.GetConnection());

            cmd.Parameters.Add("@fN", MySqlDbType.VarChar).Value = filter + "%";

            // Assign the command to the adapter
            adapter.SelectCommand = cmd;

            // Fill the DataTable with data from the database
            adapter.Fill(dt);

            // Clear the DataGridView before adding new items
            dataGridViewAttendance.DataSource = null;
            dataGridViewAttendance.Columns.Clear();

            // Set the DataSource of the DataGridView to the DataTable
            dataGridViewAttendance.DataSource = dt;

            // Adjust column headers if necessary
            dataGridViewAttendance.Columns["idAttendance"].HeaderText = "Id";
            dataGridViewAttendance.Columns["Date"].HeaderText = "Дата и время";
            dataGridViewAttendance.Columns["IsPresent"].HeaderText = "Присутствие";
            dataGridViewAttendance.Columns["studentsName"].HeaderText = "Студент";
            dataGridViewAttendance.Columns["courseName"].HeaderText = "Курс";

            dataGridViewAttendance.Columns["idAttendance"].Width = 70;
            dataGridViewAttendance.Columns["Date"].Width = 150;
            dataGridViewAttendance.Columns["IsPresent"].Width = 110;
            dataGridViewAttendance.Columns["studentsName"].Width = 170;
            dataGridViewAttendance.Columns["courseName"].Width = 400;
        }



        // Driving practices
        private void tabPageDrivingPractice_Enter(object sender, EventArgs e)
        {
            // Refresh dataTable
            textBoxDrivingPracticeFilter_TextChanged(sender, e);

            // Initialize database connection and data adapter
            DB dB = new DB();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            // DataTable for the combobox
            DataTable dtStudensNames = new DataTable();
            MySqlCommand cmdStudensNames = new MySqlCommand("SELECT `idStudents`, `Name` FROM `students`", dB.GetConnection());

            dB.openConnection();

            adapter.SelectCommand = cmdStudensNames;
            adapter.Fill(dtStudensNames);

            comboBoxDrivingPracticeStudentName.DataSource = dtStudensNames;
            comboBoxDrivingPracticeStudentName.DisplayMember = "Name";
            comboBoxDrivingPracticeStudentName.ValueMember = "idStudents";

            dB.closeConnection();
        }

        private void buttonDrivingPracticeAddList_Click(object sender, EventArgs e)
        {
            // Checking for an empty string
            if (comboBoxDrivingPracticeStudentName.Text.Length < 1 ||
                dateTimePickerDrivingPracticeDateTime.Text.Length < 1)
            {
                MessageBox.Show("Пожалуйста заполенте все поля!");
                return;
            }

            DB dB = new DB();

            int studentId = Convert.ToInt32(comboBoxDrivingPracticeStudentName.SelectedValue);
            DateTime date = Convert.ToDateTime(dateTimePickerDrivingPracticeDateTime.Value);
            string instructor = textBoxDrivingPracticeInstructorName.Text;
            double durationInHours = Convert.ToDouble(numericUpDownDrivingPracticeDurationInHours.Value);


            MySqlCommand cmd = new MySqlCommand("INSERT INTO `drivingpractice` (`Students_idStudents`, `Date`, `Instructor`, `DurationInHours`) VALUES (@sId, @Dt, @ins, @DiH)", dB.GetConnection());
            cmd.Parameters.Add("@sId", MySqlDbType.VarChar).Value = studentId;
            cmd.Parameters.AddWithValue("@Dt", date);
            cmd.Parameters.Add("@ins", MySqlDbType.VarChar).Value = instructor;
            cmd.Parameters.Add("@DiH", MySqlDbType.VarChar).Value = durationInHours;

            dB.openConnection();

            if (cmd.ExecuteNonQuery() == 1)
            {
                DataTable dt = new DataTable();
                cmd = new MySqlCommand("SELECT * FROM `drivingpractice` WHERE `idDrivingPractice` = (SELECT MAX(`idDrivingPractice`) FROM `drivingpractice`);", dB.GetConnection());
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    int drivingPracticeId = Convert.ToInt32(row["idDrivingPractice"]);
                    DrivingPractice practice = new DrivingPractice(drivingPracticeId, studentId, date, instructor, durationInHours);
                    courseAnalysis.AddDrivingPractice(studentId, practice);
                }
                MessageBox.Show("Запись добавлена!");
            }
            else
                MessageBox.Show("Ошибка при добавлении!");

            comboBoxDrivingPracticeStudentName.SelectedIndex = 0;
            textBoxDrivingPracticeInstructorName.Text = "";
            numericUpDownDrivingPracticeDurationInHours.Value = 1;

            dB.closeConnection();

            tabPageDrivingPractice_Enter(sender, e);
        }

        private void textBoxDrivingPracticeFilter_TextChanged(object sender, EventArgs e)
        {
            // Initialize database connection and data adapter
            DB dB = new DB();
            DataTable dt = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            string filter = textBoxDrivingPracticeFilter.Text;

            // Create and configure the SQL command
            MySqlCommand cmd = new MySqlCommand("SELECT `idDrivingPractice`, `students`.`Name` AS `studentName`, `Date`, `Instructor`, `DurationInHours` FROM `DrivingPractice`\r\n" +
                "LEFT JOIN `students` ON `DrivingPractice`.`Students_idStudents` = `students`.`idStudents`\r\n" +
                "WHERE `students`.`Name` LIKE @fN " +
                "ORDER BY `idDrivingPractice`;", dB.GetConnection());

            cmd.Parameters.Add("@fN", MySqlDbType.VarChar).Value = filter + "%";

            // Assign the command to the adapter
            adapter.SelectCommand = cmd;

            // Fill the DataTable with data from the database
            adapter.Fill(dt);

            // Clear the DataGridView before adding new items
            dataGridViewDrivingPractice.DataSource = null;
            dataGridViewDrivingPractice.Columns.Clear();

            // Set the DataSource of the DataGridView to the DataTable
            dataGridViewDrivingPractice.DataSource = dt;

            // Adjust column headers if necessary
            dataGridViewDrivingPractice.Columns["idDrivingPractice"].HeaderText = "Id";
            dataGridViewDrivingPractice.Columns["studentName"].HeaderText = "Студент";
            dataGridViewDrivingPractice.Columns["Date"].HeaderText = "Дата и время";
            dataGridViewDrivingPractice.Columns["Instructor"].HeaderText = "Инструктор";
            dataGridViewDrivingPractice.Columns["DurationInHours"].HeaderText = "Длит.(час)";

            dataGridViewDrivingPractice.Columns["idDrivingPractice"].Width = 70;
            dataGridViewDrivingPractice.Columns["studentName"].Width = 170;
            dataGridViewDrivingPractice.Columns["Date"].Width = 150;
            dataGridViewDrivingPractice.Columns["Instructor"].Width = 300;
            dataGridViewDrivingPractice.Columns["DurationInHours"].Width = 110;
        }



        // Driving errors
        private void tabPageDrivingErrors_Enter(object sender, EventArgs e)
        {
            // Refresh dataTable
            textBoxDrivingErrorsFilter_TextChanged(sender, e);

            // Initialize database connection and data adapter
            DB dB = new DB();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            // DataTable for the combobox
            DataTable dtStudensNames = new DataTable();
            MySqlCommand cmdStudensNames = new MySqlCommand("SELECT `idStudents`, `Name` FROM `students`", dB.GetConnection());

            dB.openConnection();

            adapter.SelectCommand = cmdStudensNames;
            adapter.Fill(dtStudensNames);

            comboBoxDrivingErrorsStudentName.DataSource = dtStudensNames;
            comboBoxDrivingErrorsStudentName.DisplayMember = "Name";
            comboBoxDrivingErrorsStudentName.ValueMember = "idStudents";

            dB.closeConnection();
        }

        private void buttonDrivingErrorsAddList_Click(object sender, EventArgs e)
        {
            // Checking for an empty string
            if (comboBoxDrivingErrorsStudentName.Text.Length < 1 ||
                dateTimePickerDrivingErrorsDateTime.Text.Length < 1)
            {
                MessageBox.Show("Пожалуйста заполенте все поля!");
                return;
            }

            DB dB = new DB();

            int studentId = Convert.ToInt32(comboBoxDrivingErrorsStudentName.SelectedValue);
            DateTime date = Convert.ToDateTime(dateTimePickerDrivingErrorsDateTime.Value);
            string description = textBoxDrivingErrorsDescription.Text;

            MySqlCommand cmd = new MySqlCommand("INSERT INTO `drivingerrors` (`Students_idStudents`, `Date`, `ErrorDescription`) VALUES (@sId, @dT, @erD);", dB.GetConnection());
            cmd.Parameters.Add("@sId", MySqlDbType.VarChar).Value = studentId;
            cmd.Parameters.AddWithValue("@Dt", date);
            cmd.Parameters.Add("@erD", MySqlDbType.VarChar).Value = description;

            dB.openConnection();

            if (cmd.ExecuteNonQuery() == 1)
            {
                DataTable dt = new DataTable();
                cmd = new MySqlCommand("SELECT * FROM `drivingerrors` WHERE `idDrivingErrors` = (SELECT MAX(`idDrivingErrors`) FROM `drivingerrors`);", dB.GetConnection());
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    int errorsId = Convert.ToInt32(row["idDrivingErrors"]);
                    DrivingErrors errors = new DrivingErrors(errorsId, studentId, date, description);
                    courseAnalysis.AddDrivingErrors(studentId, errors);
                }
                MessageBox.Show("Запись добавлена!");
            }
            else
                MessageBox.Show("Ошибка при добавлении!");

            comboBoxDrivingErrorsStudentName.SelectedIndex = 0;
            textBoxDrivingErrorsDescription.Text = "";

            dB.closeConnection();

            tabPageDrivingErrors_Enter(sender, e);
        }

        private void textBoxDrivingErrorsFilter_TextChanged(object sender, EventArgs e)
        {
            // Initialize database connection and data adapter
            DB dB = new DB();
            DataTable dt = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            string filter = textBoxDrivingErrorsFilter.Text;

            // Create and configure the SQL command
            MySqlCommand cmd = new MySqlCommand("SELECT `idDrivingErrors`, `students`.`Name` AS `studentName`, `Date`, `ErrorDescription` FROM `drivingerrors`\r\n" +
                "LEFT JOIN `students` ON `drivingerrors`.`Students_idStudents` = `students`.`idStudents`\r\n" +
                "WHERE `students`.`Name` LIKE @fN\r\n" +
                "ORDER BY `idDrivingErrors`;", dB.GetConnection());

            cmd.Parameters.Add("@fN", MySqlDbType.VarChar).Value = filter + "%";

            // Assign the command to the adapter
            adapter.SelectCommand = cmd;

            // Fill the DataTable with data from the database
            adapter.Fill(dt);

            // Clear the DataGridView before adding new items
            dataGridViewDrivingErrors.DataSource = null;
            dataGridViewDrivingErrors.Columns.Clear();

            // Set the DataSource of the DataGridView to the DataTable
            dataGridViewDrivingErrors.DataSource = dt;

            // Adjust column headers if necessary
            dataGridViewDrivingErrors.Columns["idDrivingErrors"].HeaderText = "Id";
            dataGridViewDrivingErrors.Columns["studentName"].HeaderText = "Студент";
            dataGridViewDrivingErrors.Columns["Date"].HeaderText = "Дата и время";
            dataGridViewDrivingErrors.Columns["ErrorDescription"].HeaderText = "Описание ошибки";

            dataGridViewDrivingErrors.Columns["idDrivingErrors"].Width = 70;
            dataGridViewDrivingErrors.Columns["studentName"].Width = 170;
            dataGridViewDrivingErrors.Columns["Date"].Width = 150;
            dataGridViewDrivingErrors.Columns["ErrorDescription"].Width = 500;
        }



        // Test results
        private void tabPageTestResults_Enter(object sender, EventArgs e)
        {
            // Refresh dataTable
            textBoxTestResultsFilter_TextChanged(sender, e);

            // Initialize database connection and data adapter
            DB dB = new DB();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            // DataTable for the combobox
            DataTable dtStudensNames = new DataTable();
            MySqlCommand cmdStudensNames = new MySqlCommand("SELECT `idStudents`, `Name` FROM `students`", dB.GetConnection());

            dB.openConnection();

            adapter.SelectCommand = cmdStudensNames;
            adapter.Fill(dtStudensNames);

            comboBoxTestResultsStudentName.DataSource = dtStudensNames;
            comboBoxTestResultsStudentName.DisplayMember = "Name";
            comboBoxTestResultsStudentName.ValueMember = "idStudents";

            dB.closeConnection();
        }

        private void buttonTestResults_Click(object sender, EventArgs e)
        {
            // Checking for an empty string
            if (comboBoxTestResultsStudentName.Text.Length < 1 ||
                dateTimePickerTestResultsDateTime.Text.Length < 1)
            {
                MessageBox.Show("Пожалуйста заполенте все поля!");
                return;
            }

            DB dB = new DB();

            int studentId = Convert.ToInt32(comboBoxTestResultsStudentName.SelectedValue);
            DateTime date = Convert.ToDateTime(dateTimePickerTestResultsDateTime.Value);
            double score = Convert.ToDouble(numericUpDownTestResultsScore.Value);

            MySqlCommand cmd = new MySqlCommand("INSERT INTO `testresults` (`Students_idStudents`, `Date`, `Score`) VALUES (@sId, @dT, @sc);", dB.GetConnection());
            cmd.Parameters.Add("@sId", MySqlDbType.VarChar).Value = studentId;
            cmd.Parameters.AddWithValue("@Dt", date);
            cmd.Parameters.Add("@sc", MySqlDbType.VarChar).Value = score;

            dB.openConnection();

            if (cmd.ExecuteNonQuery() == 1)
            {
                DataTable dt = new DataTable();
                cmd = new MySqlCommand("SELECT * FROM `testresults` WHERE `idTestResults` = (SELECT MAX(`idTestResults`) FROM `testresults`);", dB.GetConnection());
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    int testId = Convert.ToInt32(row["idTestResults"]);
                    TestResults test = new TestResults(testId, studentId, date, score);
                    courseAnalysis.AddTestResults(studentId, test);
                }
                MessageBox.Show("Запись добавлена!");
            }
            else
                MessageBox.Show("Ошибка при добавлении!");

            comboBoxTestResultsStudentName.SelectedIndex = 0;
            numericUpDownTestResultsScore.Value = 0;

            dB.closeConnection();

            tabPageTestResults_Enter(sender, e);
        }

        private void textBoxTestResultsFilter_TextChanged(object sender, EventArgs e)
        {
            // Initialize database connection and data adapter
            DB dB = new DB();
            DataTable dt = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            string filter = textBoxTestResultsFilter.Text;

            // Create and configure the SQL command
            MySqlCommand cmd = new MySqlCommand("SELECT `idTestResults`, `students`.`Name` AS `studentName`, `Date`, `Score` FROM `testresults`\r\n" +
                "LEFT JOIN `students` ON `testresults`.`Students_idStudents` = `students`.`idStudents`\r\n" +
                "WHERE `students`.`Name` LIKE @fN\r\n" +
                "ORDER BY `idTestResults`;", dB.GetConnection());

            cmd.Parameters.Add("@fN", MySqlDbType.VarChar).Value = filter + "%";

            // Assign the command to the adapter
            adapter.SelectCommand = cmd;

            // Fill the DataTable with data from the database
            adapter.Fill(dt);

            // Clear the DataGridView before adding new items
            dataGridViewTestResults.DataSource = null;
            dataGridViewTestResults.Columns.Clear();

            // Set the DataSource of the DataGridView to the DataTable
            dataGridViewTestResults.DataSource = dt;

            // Adjust column headers if necessary
            dataGridViewTestResults.Columns["idTestResults"].HeaderText = "Id";
            dataGridViewTestResults.Columns["studentName"].HeaderText = "Студент";
            dataGridViewTestResults.Columns["Date"].HeaderText = "Дата и время";
            dataGridViewTestResults.Columns["Score"].HeaderText = "Баллы";

            dataGridViewTestResults.Columns["idTestResults"].Width = 70;
            dataGridViewTestResults.Columns["studentName"].Width = 170;
            dataGridViewTestResults.Columns["Date"].Width = 150;
            dataGridViewTestResults.Columns["Score"].Width = 100;
        }



        // Exam results
        private void tabPageExamResults_Enter(object sender, EventArgs e)
        {
            // Refresh dataTable
            textBoxExamResultsFilter_TextChanged(sender, e);

            // Initialize database connection and data adapter
            DB dB = new DB();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            // DataTable for the combobox
            DataTable dtStudensNames = new DataTable();
            MySqlCommand cmdStudensNames = new MySqlCommand("SELECT `idStudents`, `Name` FROM `students`", dB.GetConnection());

            dB.openConnection();

            adapter.SelectCommand = cmdStudensNames;
            adapter.Fill(dtStudensNames);

            comboBoxExamResultsStudentName.DataSource = dtStudensNames;
            comboBoxExamResultsStudentName.DisplayMember = "Name";
            comboBoxExamResultsStudentName.ValueMember = "idStudents";

            dB.closeConnection();
        }

        private void buttonExamResults_Click(object sender, EventArgs e)
        {
            // Checking for an empty string
            if (comboBoxExamResultsStudentName.Text.Length < 1 ||
                dateTimePickerExamResultsDateTime.Text.Length < 1)
            {
                MessageBox.Show("Пожалуйста заполенте все поля!");
                return;
            }

            DB dB = new DB();

            int studentId = Convert.ToInt32(comboBoxExamResultsStudentName.SelectedValue);
            DateTime date = Convert.ToDateTime(dateTimePickerExamResultsDateTime.Value);
            bool isPassed = checkBoxExamResultsIsPassed.Checked;

            MySqlCommand cmd = new MySqlCommand("INSERT INTO `examresults` (`Students_idStudents`, `Date`, `IsPassed`) VALUES (@sId, @dT, @iP);", dB.GetConnection());
            cmd.Parameters.Add("@sId", MySqlDbType.VarChar).Value = studentId;
            cmd.Parameters.AddWithValue("@Dt", date);
            cmd.Parameters.Add("@iP", MySqlDbType.VarChar).Value = isPassed == true ? 1 : 0;

            dB.openConnection();

            if (cmd.ExecuteNonQuery() == 1)
            {
                DataTable dt = new DataTable();
                cmd = new MySqlCommand("SELECT * FROM `examresults` WHERE `idExamResults` = (SELECT MAX(`idExamResults`) FROM `examresults`);", dB.GetConnection());
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    int examId = Convert.ToInt32(row["idExamResults"]);
                    ExamResults exam = new ExamResults(examId, studentId, date, isPassed);
                    courseAnalysis.AddExamResults(studentId, exam);
                }
                MessageBox.Show("Запись добавлена!");
            }
            else
                MessageBox.Show("Ошибка при добавлении!");

            comboBoxExamResultsStudentName.SelectedIndex = 0;
            checkBoxExamResultsIsPassed.Checked = false;

            dB.closeConnection();

            tabPageExamResults_Enter(sender, e);
        }

        private void textBoxExamResultsFilter_TextChanged(object sender, EventArgs e)
        {
            // Initialize database connection and data adapter
            DB dB = new DB();
            DataTable dt = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            string filter = textBoxExamResultsFilter.Text;

            // Create and configure the SQL command
            MySqlCommand cmd = new MySqlCommand("SELECT `idExamResults`, `students`.`Name` AS `studentName`, `Date`, `IsPassed` FROM `examresults`\r\n" +
                "LEFT JOIN `students` ON `examresults`.`Students_idStudents` = `students`.`idStudents`\r\n" +
                "WHERE `students`.`Name` LIKE @fN\r\n" +
                "ORDER BY `idExamResults`;", dB.GetConnection());

            cmd.Parameters.Add("@fN", MySqlDbType.VarChar).Value = filter + "%";

            // Assign the command to the adapter
            adapter.SelectCommand = cmd;

            // Fill the DataTable with data from the database
            adapter.Fill(dt);

            // Clear the DataGridView before adding new items
            dataGridViewExamResults.DataSource = null;
            dataGridViewExamResults.Columns.Clear();

            // Set the DataSource of the DataGridView to the DataTable
            dataGridViewExamResults.DataSource = dt;

            // Adjust column headers if necessary
            dataGridViewExamResults.Columns["idExamResults"].HeaderText = "Id";
            dataGridViewExamResults.Columns["studentName"].HeaderText = "Студент";
            dataGridViewExamResults.Columns["Date"].HeaderText = "Дата и время";
            dataGridViewExamResults.Columns["IsPassed"].HeaderText = "Экзамен пройден";

            dataGridViewExamResults.Columns["idExamResults"].Width = 70;
            dataGridViewExamResults.Columns["studentName"].Width = 170;
            dataGridViewExamResults.Columns["Date"].Width = 150;
            dataGridViewExamResults.Columns["IsPassed"].Width = 200;
        }



        // Course analysis
        private void tabPageCourseAnalysis_Enter(object sender, EventArgs e)
        {
            textBoxCourseAnalysisFilter_TextChanged(sender, e);
        }

        private void textBoxCourseAnalysisFilter_TextChanged(object sender, EventArgs e)
        {
            // Initialize database connection and data adapter
            DB dB = new DB();
            DataTable dt = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            string filter = textBoxCourseAnalysisFilter.Text;

            // Create and configure the SQL command
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM `course` WHERE `Name` LIKE @fN;", dB.GetConnection());
            cmd.Parameters.Add("@fN", MySqlDbType.VarChar).Value = filter + "%";

            // Assign the command to the adapter
            adapter.SelectCommand = cmd;

            // Fill the DataTable with data from the database
            adapter.Fill(dt);

            // Add new columns to the DataTable
            dt.Columns.Add("Popularity", typeof(double));
            dt.Columns.Add("Quality", typeof(double));
            dt.Columns.Add("CostEffectiveness", typeof(double));

            // Clear the DataGridView before adding new items
            dataGridViewCourseAnalysis.DataSource = null;
            dataGridViewCourseAnalysis.Columns.Clear();

            // Set the DataSource of the DataGridView to the DataTable
            dataGridViewCourseAnalysis.DataSource = dt;

            // Adjust column headers if necessary
            dataGridViewCourseAnalysis.Columns["idCourse"].HeaderText = "Id";
            dataGridViewCourseAnalysis.Columns["Name"].HeaderText = "Название курса";
            dataGridViewCourseAnalysis.Columns["Cost"].HeaderText = "Стоимость";
            dataGridViewCourseAnalysis.Columns["DurationInDays"].HeaderText = "Длительность\n(дн.)";
            dataGridViewCourseAnalysis.Columns["Popularity"].HeaderText = "Популярность";
            dataGridViewCourseAnalysis.Columns["Quality"].HeaderText = "Качество";
            dataGridViewCourseAnalysis.Columns["CostEffectiveness"].HeaderText = "Эффективность Стоимости";


            dataGridViewCourseAnalysis.Columns["idCourse"].Width = 70;
            dataGridViewCourseAnalysis.Columns["Name"].Width = 330;
            dataGridViewCourseAnalysis.Columns["Cost"].Width = 120;
            dataGridViewCourseAnalysis.Columns["DurationInDays"].Width = 140;
            dataGridViewCourseAnalysis.Columns["Popularity"].Width = 150;
            dataGridViewCourseAnalysis.Columns["Quality"].Width = 120;
            dataGridViewCourseAnalysis.Columns["CostEffectiveness"].Width = 150;

            // Создаем объект анализа курсов
            foreach (DataRow row in dt.Rows)
            {
                int courseId = Convert.ToInt32(row["idCourse"]);
                string courseName = row["Name"].ToString();
                double courseCost = Convert.ToDouble(row["Cost"]);
                int courseDuration = Convert.ToInt32(row["DurationInDays"]);

                double popularity = courseAnalysis.GetCoursePopularity(courseId);
                double quality = courseAnalysis.GetCourseQuality(courseId);
                double costEffectiveness = courseAnalysis.GetCourseCostEffectiveness(courseId);

                row["Popularity"] = popularity;
                row["Quality"] = Math.Round(quality, 3);
                row["CostEffectiveness"] = Math.Round(costEffectiveness, 3);
            }

            // Обновляем DataGridView
            dataGridViewCourseAnalysis.DataSource = dt;
        }
    }
}