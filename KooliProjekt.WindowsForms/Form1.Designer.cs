namespace KooliProjekt.WindowsForms
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            label1 = new Label();
            label2 = new Label();
            idField = new TextBox();
            titleField = new TextBox();
            saveCommand = new Button();
            addCommand = new Button();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            budgetField = new TextBox();
            startDateField = new TextBox();
            priceField = new TextBox();
            dueDateField = new TextBox();
            label6 = new Label();
            deleteCommand = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Margin = new Padding(3, 2, 3, 2);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(525, 300);
            dataGridView1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(543, 18);
            label1.Name = "label1";
            label1.Size = new Size(21, 15);
            label1.TabIndex = 1;
            label1.Text = "ID:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(543, 47);
            label2.Name = "label2";
            label2.Size = new Size(42, 15);
            label2.TabIndex = 2;
            label2.Text = "Name:";
            // 
            // idField
            // 
            idField.Location = new Point(629, 15);
            idField.Name = "idField";
            idField.ReadOnly = true;
            idField.Size = new Size(100, 23);
            idField.TabIndex = 3;
            idField.Text = "-1";
            // 
            // titleField
            // 
            titleField.Location = new Point(629, 44);
            titleField.Name = "titleField";
            titleField.Size = new Size(100, 23);
            titleField.TabIndex = 4;
            // 
            // saveCommand
            // 
            saveCommand.Location = new Point(548, 225);
            saveCommand.Name = "saveCommand";
            saveCommand.Size = new Size(75, 26);
            saveCommand.TabIndex = 5;
            saveCommand.Text = "Salvesta";
            saveCommand.UseVisualStyleBackColor = true;
            saveCommand.Click += saveCommand_Click_1;
            // 
            // addCommand
            // 
            addCommand.Location = new Point(669, 225);
            addCommand.Name = "addCommand";
            addCommand.Size = new Size(75, 26);
            addCommand.TabIndex = 6;
            addCommand.Text = "Lisa uus";
            addCommand.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(543, 77);
            label3.Name = "label3";
            label3.Size = new Size(48, 15);
            label3.TabIndex = 7;
            label3.Text = "Budget:";
            label3.Click += label3_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(543, 110);
            label4.Name = "label4";
            label4.Size = new Size(61, 15);
            label4.TabIndex = 8;
            label4.Text = "DateTime:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(543, 174);
            label5.Name = "label5";
            label5.Size = new Size(80, 15);
            label5.TabIndex = 9;
            label5.Text = "PricePerHour:";
            label5.Click += label5_Click;
            // 
            // budgetField
            // 
            budgetField.Location = new Point(629, 74);
            budgetField.Name = "budgetField";
            budgetField.Size = new Size(100, 23);
            budgetField.TabIndex = 10;
            // 
            // startDateField
            // 
            startDateField.Location = new Point(629, 107);
            startDateField.Name = "startDateField";
            startDateField.Size = new Size(100, 23);
            startDateField.TabIndex = 11;
            // 
            // priceField
            // 
            priceField.Location = new Point(629, 171);
            priceField.Name = "priceField";
            priceField.Size = new Size(100, 23);
            priceField.TabIndex = 12;
            // 
            // dueDateField
            // 
            dueDateField.Location = new Point(629, 139);
            dueDateField.Name = "dueDateField";
            dueDateField.Size = new Size(100, 23);
            dueDateField.TabIndex = 13;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(543, 142);
            label6.Name = "label6";
            label6.Size = new Size(55, 15);
            label6.TabIndex = 14;
            label6.Text = "DueDate:";
            label6.Click += label6_Click;
            // 
            // deleteCommand
            // 
            deleteCommand.Location = new Point(783, 225);
            deleteCommand.Name = "deleteCommand";
            deleteCommand.Size = new Size(75, 26);
            deleteCommand.TabIndex = 15;
            deleteCommand.Text = "Kustuta";
            deleteCommand.UseVisualStyleBackColor = true;
            //deletecommand.Click += deletecommand_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(895, 338);
            Controls.Add(deleteCommand);
            Controls.Add(label6);
            Controls.Add(dueDateField);
            Controls.Add(priceField);
            Controls.Add(startDateField);
            Controls.Add(budgetField);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(addCommand);
            Controls.Add(saveCommand);
            Controls.Add(titleField);
            Controls.Add(idField);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(dataGridView1);
            Margin = new Padding(3, 2, 3, 2);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView1;
        private Label label1;
        private Label label2;
        private TextBox idField;
        private TextBox titleField;
        private Button saveCommand;
        private Button addCommand;
        private Label label3;
        private Label label4;
        private Label label5;
        private TextBox budgetField;
        private TextBox startDateField;
        private TextBox priceField;
        private TextBox dueDateField;
        private Label label6;
        private Button deleteCommand;
    }
}
