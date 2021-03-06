﻿namespace MudDesigner.Editor
{
    partial class FrmEngineSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label16 = new System.Windows.Forms.Label();
            this.btnRemoveScriptLibrary = new System.Windows.Forms.Button();
            this.btnAddScriptLibrary = new System.Windows.Forms.Button();
            this.scriptsPath = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.scriptLibrary = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.LoginCompleteState = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.loginState = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.defaultPlayerType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.defaultGameType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.doorType = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.roomType = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.zoneType = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.realmType = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.defaultWorldType = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this.playerSavePath = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.worldFile = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.btnSaveSettings = new System.Windows.Forms.Button();
            this.btnCancelSettings = new System.Windows.Forms.Button();
            this.scriptProgressBar = new System.Windows.Forms.ProgressBar();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btnSetLoginRoom = new System.Windows.Forms.Button();
            this.lblLoginRoom = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.btnRemoveScriptLibrary);
            this.groupBox1.Controls.Add(this.btnAddScriptLibrary);
            this.groupBox1.Controls.Add(this.scriptsPath);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.scriptLibrary);
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(268, 99);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(244, 155);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Script Settings";
            // 
            // label16
            // 
            this.label16.Location = new System.Drawing.Point(8, 37);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(229, 27);
            this.label16.TabIndex = 20;
            this.label16.Text = "Note: If left blank, the compiler will scan from the server root directory for sc" +
    "ripts.";
            // 
            // btnRemoveScriptLibrary
            // 
            this.btnRemoveScriptLibrary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveScriptLibrary.Location = new System.Drawing.Point(130, 67);
            this.btnRemoveScriptLibrary.Name = "btnRemoveScriptLibrary";
            this.btnRemoveScriptLibrary.Size = new System.Drawing.Size(105, 23);
            this.btnRemoveScriptLibrary.TabIndex = 13;
            this.btnRemoveScriptLibrary.Text = "Remove Library";
            this.btnRemoveScriptLibrary.UseVisualStyleBackColor = true;
            // 
            // btnAddScriptLibrary
            // 
            this.btnAddScriptLibrary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddScriptLibrary.Location = new System.Drawing.Point(6, 67);
            this.btnAddScriptLibrary.Name = "btnAddScriptLibrary";
            this.btnAddScriptLibrary.Size = new System.Drawing.Size(105, 23);
            this.btnAddScriptLibrary.TabIndex = 12;
            this.btnAddScriptLibrary.Text = "Add Script Library";
            this.btnAddScriptLibrary.UseVisualStyleBackColor = true;
            // 
            // scriptsPath
            // 
            this.scriptsPath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.scriptsPath.ForeColor = System.Drawing.Color.White;
            this.scriptsPath.Location = new System.Drawing.Point(110, 14);
            this.scriptsPath.Name = "scriptsPath";
            this.scriptsPath.Size = new System.Drawing.Size(127, 20);
            this.scriptsPath.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Script Folder Name";
            // 
            // scriptLibrary
            // 
            this.scriptLibrary.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.scriptLibrary.FormattingEnabled = true;
            this.scriptLibrary.Location = new System.Drawing.Point(3, 96);
            this.scriptLibrary.Name = "scriptLibrary";
            this.scriptLibrary.Size = new System.Drawing.Size(238, 56);
            this.scriptLibrary.Sorted = true;
            this.scriptLibrary.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.LoginCompleteState);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.loginState);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.ForeColor = System.Drawing.Color.White;
            this.groupBox2.Location = new System.Drawing.Point(12, 86);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(244, 93);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Client Login Items";
            // 
            // LoginCompleteState
            // 
            this.LoginCompleteState.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.LoginCompleteState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LoginCompleteState.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LoginCompleteState.ForeColor = System.Drawing.Color.White;
            this.LoginCompleteState.FormattingEnabled = true;
            this.LoginCompleteState.Location = new System.Drawing.Point(81, 46);
            this.LoginCompleteState.Name = "LoginCompleteState";
            this.LoginCompleteState.Size = new System.Drawing.Size(154, 21);
            this.LoginCompleteState.Sorted = true;
            this.LoginCompleteState.TabIndex = 19;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(9, 46);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 31);
            this.label7.TabIndex = 18;
            this.label7.Text = "Login Completed State";
            // 
            // loginState
            // 
            this.loginState.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.loginState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.loginState.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.loginState.ForeColor = System.Drawing.Color.White;
            this.loginState.FormattingEnabled = true;
            this.loginState.Location = new System.Drawing.Point(81, 13);
            this.loginState.Name = "loginState";
            this.loginState.Size = new System.Drawing.Size(154, 21);
            this.loginState.Sorted = true;
            this.loginState.TabIndex = 15;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Login Script";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.defaultPlayerType);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.defaultGameType);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.ForeColor = System.Drawing.Color.White;
            this.groupBox3.Location = new System.Drawing.Point(12, 9);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(244, 71);
            this.groupBox3.TabIndex = 15;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Game Defaults";
            // 
            // defaultPlayerType
            // 
            this.defaultPlayerType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.defaultPlayerType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.defaultPlayerType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.defaultPlayerType.ForeColor = System.Drawing.Color.White;
            this.defaultPlayerType.FormattingEnabled = true;
            this.defaultPlayerType.Location = new System.Drawing.Point(81, 40);
            this.defaultPlayerType.Name = "defaultPlayerType";
            this.defaultPlayerType.Size = new System.Drawing.Size(156, 21);
            this.defaultPlayerType.Sorted = true;
            this.defaultPlayerType.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Player Script";
            // 
            // defaultGameType
            // 
            this.defaultGameType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.defaultGameType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.defaultGameType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.defaultGameType.ForeColor = System.Drawing.Color.White;
            this.defaultGameType.FormattingEnabled = true;
            this.defaultGameType.Location = new System.Drawing.Point(81, 13);
            this.defaultGameType.Name = "defaultGameType";
            this.defaultGameType.Size = new System.Drawing.Size(156, 21);
            this.defaultGameType.Sorted = true;
            this.defaultGameType.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Game Script";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.doorType);
            this.groupBox4.Controls.Add(this.label11);
            this.groupBox4.Controls.Add(this.roomType);
            this.groupBox4.Controls.Add(this.label12);
            this.groupBox4.Controls.Add(this.zoneType);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.realmType);
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Controls.Add(this.defaultWorldType);
            this.groupBox4.Controls.Add(this.label10);
            this.groupBox4.ForeColor = System.Drawing.Color.White;
            this.groupBox4.Location = new System.Drawing.Point(12, 193);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(244, 169);
            this.groupBox4.TabIndex = 16;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Environment Defaults";
            // 
            // doorType
            // 
            this.doorType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.doorType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.doorType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.doorType.ForeColor = System.Drawing.Color.White;
            this.doorType.FormattingEnabled = true;
            this.doorType.Location = new System.Drawing.Point(81, 123);
            this.doorType.Name = "doorType";
            this.doorType.Size = new System.Drawing.Size(156, 21);
            this.doorType.Sorted = true;
            this.doorType.TabIndex = 15;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(8, 126);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(60, 13);
            this.label11.TabIndex = 14;
            this.label11.Text = "Door Script";
            // 
            // roomType
            // 
            this.roomType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.roomType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.roomType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.roomType.ForeColor = System.Drawing.Color.White;
            this.roomType.FormattingEnabled = true;
            this.roomType.Location = new System.Drawing.Point(81, 96);
            this.roomType.Name = "roomType";
            this.roomType.Size = new System.Drawing.Size(156, 21);
            this.roomType.Sorted = true;
            this.roomType.TabIndex = 13;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(8, 99);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(65, 13);
            this.label12.TabIndex = 12;
            this.label12.Text = "Room Script";
            // 
            // zoneType
            // 
            this.zoneType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.zoneType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.zoneType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.zoneType.ForeColor = System.Drawing.Color.White;
            this.zoneType.FormattingEnabled = true;
            this.zoneType.Location = new System.Drawing.Point(81, 67);
            this.zoneType.Name = "zoneType";
            this.zoneType.Size = new System.Drawing.Size(156, 21);
            this.zoneType.Sorted = true;
            this.zoneType.TabIndex = 11;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(8, 70);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(62, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "Zone Script";
            // 
            // realmType
            // 
            this.realmType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.realmType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.realmType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.realmType.ForeColor = System.Drawing.Color.White;
            this.realmType.FormattingEnabled = true;
            this.realmType.Location = new System.Drawing.Point(81, 40);
            this.realmType.Name = "realmType";
            this.realmType.Size = new System.Drawing.Size(156, 21);
            this.realmType.Sorted = true;
            this.realmType.TabIndex = 9;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 43);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(67, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "Realm Script";
            // 
            // defaultWorldType
            // 
            this.defaultWorldType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.defaultWorldType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.defaultWorldType.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.defaultWorldType.ForeColor = System.Drawing.Color.White;
            this.defaultWorldType.FormattingEnabled = true;
            this.defaultWorldType.Location = new System.Drawing.Point(81, 13);
            this.defaultWorldType.Name = "defaultWorldType";
            this.defaultWorldType.Size = new System.Drawing.Size(156, 21);
            this.defaultWorldType.Sorted = true;
            this.defaultWorldType.TabIndex = 7;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(8, 16);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 13);
            this.label10.TabIndex = 6;
            this.label10.Text = "World";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label15);
            this.groupBox5.Controls.Add(this.playerSavePath);
            this.groupBox5.Controls.Add(this.label14);
            this.groupBox5.Controls.Add(this.worldFile);
            this.groupBox5.Controls.Add(this.label13);
            this.groupBox5.ForeColor = System.Drawing.Color.White;
            this.groupBox5.Location = new System.Drawing.Point(268, 260);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(244, 105);
            this.groupBox5.TabIndex = 17;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Save Information";
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(8, 70);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(229, 31);
            this.label15.TabIndex = 19;
            this.label15.Text = "Note: Leaving blank will save the files into the root server path.";
            // 
            // playerSavePath
            // 
            this.playerSavePath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.playerSavePath.ForeColor = System.Drawing.Color.White;
            this.playerSavePath.Location = new System.Drawing.Point(111, 46);
            this.playerSavePath.Name = "playerSavePath";
            this.playerSavePath.Size = new System.Drawing.Size(127, 20);
            this.playerSavePath.TabIndex = 15;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(8, 48);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(96, 13);
            this.label14.TabIndex = 14;
            this.label14.Text = "Player Save Folder";
            // 
            // worldFile
            // 
            this.worldFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.worldFile.ForeColor = System.Drawing.Color.White;
            this.worldFile.Location = new System.Drawing.Point(111, 19);
            this.worldFile.Name = "worldFile";
            this.worldFile.Size = new System.Drawing.Size(127, 20);
            this.worldFile.TabIndex = 13;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(8, 21);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(95, 13);
            this.label13.TabIndex = 12;
            this.label13.Text = "World Save Folder";
            // 
            // btnSaveSettings
            // 
            this.btnSaveSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveSettings.Location = new System.Drawing.Point(12, 368);
            this.btnSaveSettings.Name = "btnSaveSettings";
            this.btnSaveSettings.Size = new System.Drawing.Size(244, 23);
            this.btnSaveSettings.TabIndex = 20;
            this.btnSaveSettings.Text = "Save Settings";
            this.btnSaveSettings.UseVisualStyleBackColor = true;
            this.btnSaveSettings.Click += new System.EventHandler(this.btnSaveSettings_Click);
            // 
            // btnCancelSettings
            // 
            this.btnCancelSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelSettings.Location = new System.Drawing.Point(268, 368);
            this.btnCancelSettings.Name = "btnCancelSettings";
            this.btnCancelSettings.Size = new System.Drawing.Size(244, 23);
            this.btnCancelSettings.TabIndex = 21;
            this.btnCancelSettings.Text = "Cancel";
            this.btnCancelSettings.UseVisualStyleBackColor = true;
            this.btnCancelSettings.Click += new System.EventHandler(this.btnCancelSettings_Click);
            // 
            // scriptProgressBar
            // 
            this.scriptProgressBar.Location = new System.Drawing.Point(12, 397);
            this.scriptProgressBar.Maximum = 14;
            this.scriptProgressBar.Name = "scriptProgressBar";
            this.scriptProgressBar.Size = new System.Drawing.Size(500, 23);
            this.scriptProgressBar.Step = 1;
            this.scriptProgressBar.TabIndex = 22;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.btnSetLoginRoom);
            this.groupBox6.Controls.Add(this.lblLoginRoom);
            this.groupBox6.ForeColor = System.Drawing.Color.White;
            this.groupBox6.Location = new System.Drawing.Point(268, 9);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(244, 84);
            this.groupBox6.TabIndex = 23;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Game Defaults";
            // 
            // btnSetLoginRoom
            // 
            this.btnSetLoginRoom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSetLoginRoom.Location = new System.Drawing.Point(6, 55);
            this.btnSetLoginRoom.Name = "btnSetLoginRoom";
            this.btnSetLoginRoom.Size = new System.Drawing.Size(231, 23);
            this.btnSetLoginRoom.TabIndex = 18;
            this.btnSetLoginRoom.Text = "Set Starting Room";
            this.btnSetLoginRoom.UseVisualStyleBackColor = true;
            this.btnSetLoginRoom.Click += new System.EventHandler(this.btnSetLoginRoom_Click);
            // 
            // lblLoginRoom
            // 
            this.lblLoginRoom.Location = new System.Drawing.Point(6, 16);
            this.lblLoginRoom.Name = "lblLoginRoom";
            this.lblLoginRoom.Size = new System.Drawing.Size(229, 36);
            this.lblLoginRoom.TabIndex = 17;
            this.lblLoginRoom.Text = "Login Room: None Set.";
            // 
            // frmEngineSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(520, 426);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.scriptProgressBar);
            this.Controls.Add(this.btnCancelSettings);
            this.Controls.Add(this.btnSaveSettings);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmEngineSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mud Designer Editor : Engine Settings";
            this.Load += new System.EventHandler(this.frmEngineSettings_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox scriptLibrary;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox loginState;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox LoginCompleteState;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox defaultPlayerType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox defaultGameType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox scriptsPath;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnRemoveScriptLibrary;
        private System.Windows.Forms.Button btnAddScriptLibrary;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox doorType;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox roomType;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox zoneType;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox realmType;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox defaultWorldType;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TextBox playerSavePath;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox worldFile;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button btnSaveSettings;
        private System.Windows.Forms.Button btnCancelSettings;
        private System.Windows.Forms.ProgressBar scriptProgressBar;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button btnSetLoginRoom;
        private System.Windows.Forms.Label lblLoginRoom;
    }
}