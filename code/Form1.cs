// -------------------------------------------------
// Stock Market Visual Explorer
//
// License:
// Copyright (c) 2008-2020 Joseph True
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//
// -------------------------------------------------
// Author:			      Joseph True
// Original date:         Originally created as a final project for WPI CS525D
//                        Data Visualization, Fall 2008 graduate course
//
// Programming Language:  C#
//
// Overall Design:
// An interactive data viz mashup of data and information from the 2007-2009 Financial Crisis.		
// Stock chart using candlestick and other type glyphs to plot stock prices using four different types of glyphs.
//	1. High Low
//	2. Open High Low Close
//	3. Candlestick
//	4. One that I made up
//
// Data soucres:
// Yahoo Finance
// Price history: https://finance.yahoo.com/quote/%5EDJI/history/
// DOW http://finance.yahoo.com/echarts?s=%5EDJI#chart9:symbol=^dji;range=1m;indicator=volume;charttype=candlestick;crosshair=on;ohlcvalues=0;logscale=on;source=undefined
// Federal Reserve speechs: https://www.federalreserve.gov/newsevents/speeches.htm
//
//Additional Files:		Stock data in CSV text files, plus image files in sub-dir content_files
//
// -------------------------------------------------
//
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;

namespace WindowsApplication2
{
	// .NET genertared code for form
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		//---------------------------------------
		// Application variables
		//---------------------------------------
				
		// String data values read from file
		ArrayList m_array1 = new ArrayList();
		ArrayList m_array2 = new ArrayList();
		ArrayList m_array3 = new ArrayList();
		ArrayList m_array4 = new ArrayList();
		ArrayList m_array5 = new ArrayList();
		ArrayList m_array6 = new ArrayList();
		ArrayList m_array7 = new ArrayList();
		ArrayList m_array8 = new ArrayList();

		ArrayList m_FileNames = new ArrayList();

        // Sub-directory for data and image files
        string m_contentDir = "content_files";

		// This struct will hold one day of stock price data
		public struct StockInfo
		{
			public float PriceOpen;
			public float PriceClose;
			public float PriceHigh;
			public float PriceLow;
			public string Name;
			public string Symbol;
			public string Date;
		}

		StockInfo[] m_StockData;

		// This struct will hold the speeches
		public struct FedSpeech
		{
			public string Title;
			public string Link;
			public string Date;
			public string ImageFile1;
			public string ImageFile2;
			public string ImageFile3;
			public int EventDay;
			public int EventWeek;
			public int EventMonth;
		}

		FedSpeech[] m_FedSpeeches;

		int m_speechID;	// Keeps track of currently selected speech

		// This struct will hold news events
		public struct NewsEvent
		{
			public string Title;
			public string Link;
			public string Date;
			public string ImageFile;
			public int EventDay;
			public int EventWeek;
			public int EventMonth;
		}

		NewsEvent[] m_NewsEvents;



		// Axis points
		static int m_ChartWidth = 1060;	// chart width
		static int m_offsetX = 46;
		static int m_offsetY = 340;	// top of y axis
		
		static int m_xAxisStart = m_offsetX;
		static int m_yAxisStart = m_offsetY;
		static int m_yAxisEnd =120;	// top of chart
		static int m_pAxisInterval = 40;
		static int m_yAxisHeight = m_yAxisStart - m_yAxisEnd;	// height of chart
		
		string[] m_DimNames = new string[8];

		//imported data
		float[,] m_XYdata;
		float m_yMin; // = new float;
		float m_yMax;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.ComboBox cboStockName;
		private System.Windows.Forms.RadioButton rdbCandlestick;
		private System.Windows.Forms.RadioButton rdbHighLow;
		private System.Windows.Forms.RadioButton rdbMyGlyph;
		private System.Windows.Forms.RadioButton rdbOHLC;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.PictureBox pictureBox2;
		private System.Windows.Forms.PictureBox pictureBox3;
		private System.Windows.Forms.PictureBox pictureBox4;
		private System.Windows.Forms.PictureBox pictureBox5;
		private System.ComponentModel.IContainer components;

		private string[] m_myImages = new string[11];
		private System.Windows.Forms.PictureBox pictureBox6;
		private System.Windows.Forms.PictureBox pictureBox7;
		private System.Windows.Forms.PictureBox pictureBox8;
		private System.Windows.Forms.PictureBox pictureBox9;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.LinkLabel linkLabel1;
		private System.Windows.Forms.RadioButton rdoLine;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.Label lblSpeechDate1;
		private System.Windows.Forms.Label lblSpeechDate2;
		private System.Windows.Forms.Label lblSpeechDate3;
		private System.Windows.Forms.Label lblSpeechDate4;
		private System.Windows.Forms.Label lblSpeechDate5;
		private System.Windows.Forms.Label lblSpeechDate6;
		private System.Windows.Forms.Label lblSpeechDate7;
		private System.Windows.Forms.Label lblSpeechDate8;
		private System.Windows.Forms.Label lblSpeechDate9;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.CheckBox chkShowMinMax;
		private System.Windows.Forms.CheckBox checkBox3;
		private System.Windows.Forms.GroupBox grpSpeech;
		private System.Windows.Forms.PictureBox picSpeech1;
		private System.Windows.Forms.PictureBox picSpeech2;
		private System.Windows.Forms.CheckBox chkShowNewsEvents;
		private System.Windows.Forms.PictureBox picLegendLine;
		private System.Windows.Forms.PictureBox picLegendHighLow;
		private System.Windows.Forms.PictureBox picLegendOHLC;
		private System.Windows.Forms.PictureBox picLegendCandlestick;
		private System.Windows.Forms.PictureBox picEventBearStearns;
		private System.Windows.Forms.PictureBox picEventFannieFreddie;
		private System.Windows.Forms.PictureBox picEventLehmanBros;
		private System.Windows.Forms.PictureBox picEventAIG;
        private Label lblWordCloudTip;
        private Label lblTitle;
        private PictureBox picCustomGlyph;
		private int[] m_myEvents = new int[10];

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			setImages();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cboStockName = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.picCustomGlyph = new System.Windows.Forms.PictureBox();
            this.picLegendCandlestick = new System.Windows.Forms.PictureBox();
            this.picLegendOHLC = new System.Windows.Forms.PictureBox();
            this.picLegendHighLow = new System.Windows.Forms.PictureBox();
            this.picLegendLine = new System.Windows.Forms.PictureBox();
            this.rdbMyGlyph = new System.Windows.Forms.RadioButton();
            this.rdbCandlestick = new System.Windows.Forms.RadioButton();
            this.rdbOHLC = new System.Windows.Forms.RadioButton();
            this.rdbHighLow = new System.Windows.Forms.RadioButton();
            this.rdoLine = new System.Windows.Forms.RadioButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.pictureBox7 = new System.Windows.Forms.PictureBox();
            this.pictureBox8 = new System.Windows.Forms.PictureBox();
            this.pictureBox9 = new System.Windows.Forms.PictureBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.picSpeech1 = new System.Windows.Forms.PictureBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.chkShowNewsEvents = new System.Windows.Forms.CheckBox();
            this.lblSpeechDate1 = new System.Windows.Forms.Label();
            this.lblSpeechDate2 = new System.Windows.Forms.Label();
            this.lblSpeechDate3 = new System.Windows.Forms.Label();
            this.lblSpeechDate4 = new System.Windows.Forms.Label();
            this.lblSpeechDate5 = new System.Windows.Forms.Label();
            this.lblSpeechDate6 = new System.Windows.Forms.Label();
            this.lblSpeechDate7 = new System.Windows.Forms.Label();
            this.lblSpeechDate8 = new System.Windows.Forms.Label();
            this.lblSpeechDate9 = new System.Windows.Forms.Label();
            this.chkShowMinMax = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.picEventBearStearns = new System.Windows.Forms.PictureBox();
            this.grpSpeech = new System.Windows.Forms.GroupBox();
            this.lblWordCloudTip = new System.Windows.Forms.Label();
            this.picSpeech2 = new System.Windows.Forms.PictureBox();
            this.picEventFannieFreddie = new System.Windows.Forms.PictureBox();
            this.picEventLehmanBros = new System.Windows.Forms.PictureBox();
            this.picEventAIG = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCustomGlyph)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLegendCandlestick)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLegendOHLC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLegendHighLow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLegendLine)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSpeech1)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picEventBearStearns)).BeginInit();
            this.grpSpeech.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSpeech2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEventFannieFreddie)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEventLehmanBros)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEventAIG)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cboStockName);
            this.groupBox1.Location = new System.Drawing.Point(1119, 51);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(213, 64);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Stock Market Index";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // cboStockName
            // 
            this.cboStockName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStockName.Location = new System.Drawing.Point(8, 16);
            this.cboStockName.Name = "cboStockName";
            this.cboStockName.Size = new System.Drawing.Size(199, 21);
            this.cboStockName.TabIndex = 3;
            this.cboStockName.SelectedIndexChanged += new System.EventHandler(this.cboStockName_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.picCustomGlyph);
            this.groupBox2.Controls.Add(this.picLegendCandlestick);
            this.groupBox2.Controls.Add(this.picLegendOHLC);
            this.groupBox2.Controls.Add(this.picLegendHighLow);
            this.groupBox2.Controls.Add(this.picLegendLine);
            this.groupBox2.Controls.Add(this.rdbMyGlyph);
            this.groupBox2.Controls.Add(this.rdbCandlestick);
            this.groupBox2.Controls.Add(this.rdbOHLC);
            this.groupBox2.Controls.Add(this.rdbHighLow);
            this.groupBox2.Controls.Add(this.rdoLine);
            this.groupBox2.Location = new System.Drawing.Point(1119, 267);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(213, 244);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Stock Price Line and Marker Type";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // picCustomGlyph
            // 
            this.picCustomGlyph.Location = new System.Drawing.Point(118, 186);
            this.picCustomGlyph.Name = "picCustomGlyph";
            this.picCustomGlyph.Size = new System.Drawing.Size(61, 45);
            this.picCustomGlyph.TabIndex = 11;
            this.picCustomGlyph.TabStop = false;
            // 
            // picLegendCandlestick
            // 
            this.picLegendCandlestick.Location = new System.Drawing.Point(118, 146);
            this.picLegendCandlestick.Name = "picLegendCandlestick";
            this.picLegendCandlestick.Size = new System.Drawing.Size(61, 32);
            this.picLegendCandlestick.TabIndex = 10;
            this.picLegendCandlestick.TabStop = false;
            // 
            // picLegendOHLC
            // 
            this.picLegendOHLC.Location = new System.Drawing.Point(118, 104);
            this.picLegendOHLC.Name = "picLegendOHLC";
            this.picLegendOHLC.Size = new System.Drawing.Size(61, 32);
            this.picLegendOHLC.TabIndex = 9;
            this.picLegendOHLC.TabStop = false;
            // 
            // picLegendHighLow
            // 
            this.picLegendHighLow.Location = new System.Drawing.Point(118, 62);
            this.picLegendHighLow.Name = "picLegendHighLow";
            this.picLegendHighLow.Size = new System.Drawing.Size(61, 32);
            this.picLegendHighLow.TabIndex = 8;
            this.picLegendHighLow.TabStop = false;
            // 
            // picLegendLine
            // 
            this.picLegendLine.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.picLegendLine.Location = new System.Drawing.Point(118, 29);
            this.picLegendLine.Name = "picLegendLine";
            this.picLegendLine.Size = new System.Drawing.Size(61, 19);
            this.picLegendLine.TabIndex = 7;
            this.picLegendLine.TabStop = false;
            // 
            // rdbMyGlyph
            // 
            this.rdbMyGlyph.Location = new System.Drawing.Point(16, 194);
            this.rdbMyGlyph.Name = "rdbMyGlyph";
            this.rdbMyGlyph.Size = new System.Drawing.Size(96, 24);
            this.rdbMyGlyph.TabIndex = 5;
            this.rdbMyGlyph.Text = "Custom Glyph";
            this.rdbMyGlyph.CheckedChanged += new System.EventHandler(this.rdbMyGlyph_CheckedChanged);
            // 
            // rdbCandlestick
            // 
            this.rdbCandlestick.Location = new System.Drawing.Point(16, 154);
            this.rdbCandlestick.Name = "rdbCandlestick";
            this.rdbCandlestick.Size = new System.Drawing.Size(88, 24);
            this.rdbCandlestick.TabIndex = 2;
            this.rdbCandlestick.Text = "Candlestick";
            this.rdbCandlestick.CheckedChanged += new System.EventHandler(this.rdbCandlestick_CheckedChanged);
            // 
            // rdbOHLC
            // 
            this.rdbOHLC.Location = new System.Drawing.Point(16, 104);
            this.rdbOHLC.Name = "rdbOHLC";
            this.rdbOHLC.Size = new System.Drawing.Size(90, 40);
            this.rdbOHLC.TabIndex = 1;
            this.rdbOHLC.Text = "Open, High, Low, Close";
            this.rdbOHLC.CheckedChanged += new System.EventHandler(this.rdbOHLC_CheckedChanged);
            // 
            // rdbHighLow
            // 
            this.rdbHighLow.Location = new System.Drawing.Point(16, 64);
            this.rdbHighLow.Name = "rdbHighLow";
            this.rdbHighLow.Size = new System.Drawing.Size(96, 24);
            this.rdbHighLow.TabIndex = 0;
            this.rdbHighLow.Text = "High Low Bar";
            this.rdbHighLow.CheckedChanged += new System.EventHandler(this.rdbHighLow_CheckedChanged);
            // 
            // rdoLine
            // 
            this.rdoLine.Checked = true;
            this.rdoLine.Location = new System.Drawing.Point(16, 24);
            this.rdoLine.Name = "rdoLine";
            this.rdoLine.Size = new System.Drawing.Size(56, 24);
            this.rdoLine.TabIndex = 6;
            this.rdoLine.TabStop = true;
            this.rdoLine.Text = "Line";
            this.rdoLine.CheckedChanged += new System.EventHandler(this.rdoLine_CheckedChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(120, 64);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(60, 40);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.White;
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.Location = new System.Drawing.Point(186, 64);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(60, 40);
            this.pictureBox2.TabIndex = 6;
            this.pictureBox2.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox2, "Hello World!");
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.White;
            this.pictureBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox3.Location = new System.Drawing.Point(252, 64);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(60, 40);
            this.pictureBox3.TabIndex = 7;
            this.pictureBox3.TabStop = false;
            this.pictureBox3.Click += new System.EventHandler(this.pictureBox3_Click);
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackColor = System.Drawing.Color.White;
            this.pictureBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox4.Location = new System.Drawing.Point(315, 64);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(60, 40);
            this.pictureBox4.TabIndex = 8;
            this.pictureBox4.TabStop = false;
            this.pictureBox4.Click += new System.EventHandler(this.pictureBox4_Click);
            this.pictureBox4.MouseEnter += new System.EventHandler(this.pictureBox4_MouseEnter);
            this.pictureBox4.MouseLeave += new System.EventHandler(this.pictureBox4_MouseLeave);
            // 
            // pictureBox5
            // 
            this.pictureBox5.BackColor = System.Drawing.Color.White;
            this.pictureBox5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox5.Location = new System.Drawing.Point(381, 64);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(60, 40);
            this.pictureBox5.TabIndex = 9;
            this.pictureBox5.TabStop = false;
            this.pictureBox5.Click += new System.EventHandler(this.pictureBox5_Click);
            this.pictureBox5.MouseHover += new System.EventHandler(this.pictureBox5_MouseHover);
            // 
            // pictureBox6
            // 
            this.pictureBox6.BackColor = System.Drawing.Color.White;
            this.pictureBox6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox6.Location = new System.Drawing.Point(475, 64);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(60, 40);
            this.pictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox6.TabIndex = 11;
            this.pictureBox6.TabStop = false;
            this.pictureBox6.Click += new System.EventHandler(this.pictureBox6_Click);
            // 
            // pictureBox7
            // 
            this.pictureBox7.BackColor = System.Drawing.Color.White;
            this.pictureBox7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox7.Location = new System.Drawing.Point(565, 64);
            this.pictureBox7.Name = "pictureBox7";
            this.pictureBox7.Size = new System.Drawing.Size(60, 40);
            this.pictureBox7.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox7.TabIndex = 12;
            this.pictureBox7.TabStop = false;
            this.pictureBox7.Click += new System.EventHandler(this.pictureBox7_Click);
            // 
            // pictureBox8
            // 
            this.pictureBox8.BackColor = System.Drawing.Color.White;
            this.pictureBox8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox8.Location = new System.Drawing.Point(631, 64);
            this.pictureBox8.Name = "pictureBox8";
            this.pictureBox8.Size = new System.Drawing.Size(60, 40);
            this.pictureBox8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox8.TabIndex = 13;
            this.pictureBox8.TabStop = false;
            this.pictureBox8.Click += new System.EventHandler(this.pictureBox8_Click);
            // 
            // pictureBox9
            // 
            this.pictureBox9.BackColor = System.Drawing.Color.White;
            this.pictureBox9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox9.Location = new System.Drawing.Point(697, 64);
            this.pictureBox9.Name = "pictureBox9";
            this.pictureBox9.Size = new System.Drawing.Size(60, 40);
            this.pictureBox9.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox9.TabIndex = 14;
            this.pictureBox9.TabStop = false;
            this.pictureBox9.Click += new System.EventHandler(this.pictureBox9_Click);
            // 
            // picSpeech1
            // 
            this.picSpeech1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picSpeech1.Location = new System.Drawing.Point(164, 42);
            this.picSpeech1.Name = "picSpeech1";
            this.picSpeech1.Size = new System.Drawing.Size(360, 224);
            this.picSpeech1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picSpeech1.TabIndex = 18;
            this.picSpeech1.TabStop = false;
            // 
            // linkLabel1
            // 
            this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel1.Location = new System.Drawing.Point(922, 16);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(112, 24);
            this.linkLabel1.TabIndex = 19;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "View Larger Image";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // checkBox1
            // 
            this.checkBox1.Location = new System.Drawing.Point(13, 90);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(183, 16);
            this.checkBox1.TabIndex = 20;
            this.checkBox1.Text = "Federal Reserve interest rates";
            // 
            // chkShowNewsEvents
            // 
            this.chkShowNewsEvents.Location = new System.Drawing.Point(13, 68);
            this.chkShowNewsEvents.Name = "chkShowNewsEvents";
            this.chkShowNewsEvents.Size = new System.Drawing.Size(136, 16);
            this.chkShowNewsEvents.TabIndex = 21;
            this.chkShowNewsEvents.Text = "News events";
            this.chkShowNewsEvents.CheckedChanged += new System.EventHandler(this.chkShowNewsEvents_CheckedChanged);
            // 
            // lblSpeechDate1
            // 
            this.lblSpeechDate1.BackColor = System.Drawing.Color.White;
            this.lblSpeechDate1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSpeechDate1.Location = new System.Drawing.Point(113, 107);
            this.lblSpeechDate1.Name = "lblSpeechDate1";
            this.lblSpeechDate1.Size = new System.Drawing.Size(72, 10);
            this.lblSpeechDate1.TabIndex = 22;
            this.lblSpeechDate1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSpeechDate2
            // 
            this.lblSpeechDate2.BackColor = System.Drawing.Color.White;
            this.lblSpeechDate2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSpeechDate2.Location = new System.Drawing.Point(177, 107);
            this.lblSpeechDate2.Name = "lblSpeechDate2";
            this.lblSpeechDate2.Size = new System.Drawing.Size(72, 10);
            this.lblSpeechDate2.TabIndex = 23;
            this.lblSpeechDate2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSpeechDate3
            // 
            this.lblSpeechDate3.BackColor = System.Drawing.Color.White;
            this.lblSpeechDate3.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSpeechDate3.Location = new System.Drawing.Point(241, 107);
            this.lblSpeechDate3.Name = "lblSpeechDate3";
            this.lblSpeechDate3.Size = new System.Drawing.Size(72, 10);
            this.lblSpeechDate3.TabIndex = 24;
            this.lblSpeechDate3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSpeechDate4
            // 
            this.lblSpeechDate4.BackColor = System.Drawing.Color.White;
            this.lblSpeechDate4.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSpeechDate4.Location = new System.Drawing.Point(313, 107);
            this.lblSpeechDate4.Name = "lblSpeechDate4";
            this.lblSpeechDate4.Size = new System.Drawing.Size(64, 10);
            this.lblSpeechDate4.TabIndex = 25;
            this.lblSpeechDate4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSpeechDate5
            // 
            this.lblSpeechDate5.BackColor = System.Drawing.Color.White;
            this.lblSpeechDate5.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSpeechDate5.Location = new System.Drawing.Point(379, 107);
            this.lblSpeechDate5.Name = "lblSpeechDate5";
            this.lblSpeechDate5.Size = new System.Drawing.Size(72, 10);
            this.lblSpeechDate5.TabIndex = 26;
            this.lblSpeechDate5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSpeechDate6
            // 
            this.lblSpeechDate6.BackColor = System.Drawing.Color.White;
            this.lblSpeechDate6.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSpeechDate6.Location = new System.Drawing.Point(466, 107);
            this.lblSpeechDate6.Name = "lblSpeechDate6";
            this.lblSpeechDate6.Size = new System.Drawing.Size(72, 10);
            this.lblSpeechDate6.TabIndex = 27;
            this.lblSpeechDate6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSpeechDate7
            // 
            this.lblSpeechDate7.BackColor = System.Drawing.Color.White;
            this.lblSpeechDate7.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSpeechDate7.Location = new System.Drawing.Point(559, 107);
            this.lblSpeechDate7.Name = "lblSpeechDate7";
            this.lblSpeechDate7.Size = new System.Drawing.Size(72, 10);
            this.lblSpeechDate7.TabIndex = 28;
            this.lblSpeechDate7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSpeechDate8
            // 
            this.lblSpeechDate8.BackColor = System.Drawing.Color.White;
            this.lblSpeechDate8.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSpeechDate8.Location = new System.Drawing.Point(626, 107);
            this.lblSpeechDate8.Name = "lblSpeechDate8";
            this.lblSpeechDate8.Size = new System.Drawing.Size(72, 10);
            this.lblSpeechDate8.TabIndex = 29;
            this.lblSpeechDate8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSpeechDate9
            // 
            this.lblSpeechDate9.BackColor = System.Drawing.Color.White;
            this.lblSpeechDate9.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSpeechDate9.Location = new System.Drawing.Point(694, 107);
            this.lblSpeechDate9.Name = "lblSpeechDate9";
            this.lblSpeechDate9.Size = new System.Drawing.Size(64, 10);
            this.lblSpeechDate9.TabIndex = 30;
            this.lblSpeechDate9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkShowMinMax
            // 
            this.chkShowMinMax.Location = new System.Drawing.Point(13, 24);
            this.chkShowMinMax.Name = "chkShowMinMax";
            this.chkShowMinMax.Size = new System.Drawing.Size(183, 16);
            this.chkShowMinMax.TabIndex = 34;
            this.chkShowMinMax.Text = "Stock Market Min and Max";
            this.chkShowMinMax.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.checkBox3);
            this.groupBox5.Controls.Add(this.checkBox1);
            this.groupBox5.Controls.Add(this.chkShowNewsEvents);
            this.groupBox5.Controls.Add(this.chkShowMinMax);
            this.groupBox5.Location = new System.Drawing.Point(1119, 131);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(213, 122);
            this.groupBox5.TabIndex = 35;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Chart Options";
            this.groupBox5.Enter += new System.EventHandler(this.groupBox5_Enter);
            // 
            // checkBox3
            // 
            this.checkBox3.Location = new System.Drawing.Point(13, 46);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(183, 16);
            this.checkBox3.TabIndex = 35;
            this.checkBox3.Text = "Federal Reserve Speeches";
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged_1);
            // 
            // picEventBearStearns
            // 
            this.picEventBearStearns.Location = new System.Drawing.Point(322, 231);
            this.picEventBearStearns.Name = "picEventBearStearns";
            this.picEventBearStearns.Size = new System.Drawing.Size(80, 32);
            this.picEventBearStearns.TabIndex = 36;
            this.picEventBearStearns.TabStop = false;
            // 
            // grpSpeech
            // 
            this.grpSpeech.Controls.Add(this.lblWordCloudTip);
            this.grpSpeech.Controls.Add(this.picSpeech2);
            this.grpSpeech.Controls.Add(this.picSpeech1);
            this.grpSpeech.Controls.Add(this.linkLabel1);
            this.grpSpeech.Location = new System.Drawing.Point(48, 431);
            this.grpSpeech.Name = "grpSpeech";
            this.grpSpeech.Size = new System.Drawing.Size(1050, 272);
            this.grpSpeech.TabIndex = 38;
            this.grpSpeech.TabStop = false;
            this.grpSpeech.Text = "Federal Reserve Speeches";
            // 
            // lblWordCloudTip
            // 
            this.lblWordCloudTip.AutoSize = true;
            this.lblWordCloudTip.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWordCloudTip.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblWordCloudTip.Location = new System.Drawing.Point(6, 16);
            this.lblWordCloudTip.Name = "lblWordCloudTip";
            this.lblWordCloudTip.Size = new System.Drawing.Size(220, 13);
            this.lblWordCloudTip.TabIndex = 21;
            this.lblWordCloudTip.Text = "Click a speech above to see the word clouds";
            // 
            // picSpeech2
            // 
            this.picSpeech2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picSpeech2.Location = new System.Drawing.Point(540, 42);
            this.picSpeech2.Name = "picSpeech2";
            this.picSpeech2.Size = new System.Drawing.Size(344, 224);
            this.picSpeech2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picSpeech2.TabIndex = 20;
            this.picSpeech2.TabStop = false;
            // 
            // picEventFannieFreddie
            // 
            this.picEventFannieFreddie.Location = new System.Drawing.Point(580, 276);
            this.picEventFannieFreddie.Name = "picEventFannieFreddie";
            this.picEventFannieFreddie.Size = new System.Drawing.Size(80, 56);
            this.picEventFannieFreddie.TabIndex = 39;
            this.picEventFannieFreddie.TabStop = false;
            // 
            // picEventLehmanBros
            // 
            this.picEventLehmanBros.Location = new System.Drawing.Point(577, 131);
            this.picEventLehmanBros.Name = "picEventLehmanBros";
            this.picEventLehmanBros.Size = new System.Drawing.Size(96, 32);
            this.picEventLehmanBros.TabIndex = 40;
            this.picEventLehmanBros.TabStop = false;
            // 
            // picEventAIG
            // 
            this.picEventAIG.Location = new System.Drawing.Point(655, 175);
            this.picEventAIG.Name = "picEventAIG";
            this.picEventAIG.Size = new System.Drawing.Size(72, 40);
            this.picEventAIG.TabIndex = 41;
            this.picEventAIG.TabStop = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(276, 24);
            this.lblTitle.TabIndex = 42;
            this.lblTitle.Text = "Financial Crisis of 2007-2009";
            this.lblTitle.Click += new System.EventHandler(this.label1_Click);
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1344, 710);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.picEventAIG);
            this.Controls.Add(this.picEventLehmanBros);
            this.Controls.Add(this.picEventFannieFreddie);
            this.Controls.Add(this.grpSpeech);
            this.Controls.Add(this.picEventBearStearns);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.pictureBox8);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblSpeechDate3);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.lblSpeechDate4);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.pictureBox5);
            this.Controls.Add(this.lblSpeechDate1);
            this.Controls.Add(this.lblSpeechDate2);
            this.Controls.Add(this.pictureBox6);
            this.Controls.Add(this.pictureBox7);
            this.Controls.Add(this.lblSpeechDate5);
            this.Controls.Add(this.lblSpeechDate6);
            this.Controls.Add(this.lblSpeechDate7);
            this.Controls.Add(this.pictureBox9);
            this.Controls.Add(this.lblSpeechDate8);
            this.Controls.Add(this.lblSpeechDate9);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Stock Market Visual Explorer - Financial Crisis of 2007-2009";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picCustomGlyph)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLegendCandlestick)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLegendOHLC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLegendHighLow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLegendLine)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSpeech1)).EndInit();
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picEventBearStearns)).EndInit();
            this.grpSpeech.ResumeLayout(false);
            this.grpSpeech.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSpeech2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEventFannieFreddie)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEventLehmanBros)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEventAIG)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		// --- JTrue --------------------------------------------
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());

			// UpdateDrawing();



		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
			getFileList();

			// Dimension names.
			// Date,Open,High,Low,Close,Volume,Adj Close
			m_DimNames[0]="Date";	//2008-10-31
			m_DimNames[1]="Open";
			m_DimNames[2]="High";
			m_DimNames[3]="Low";
			m_DimNames[4]="Close";
			m_DimNames[5]="Volume";
			m_DimNames[6]="Adj Close";
			//m_DimNames[7]="Weight";


            checkBox3.Checked = true;

			pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle ;
			pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle ;
			pictureBox3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle ;
			pictureBox4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle ;
			pictureBox5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle ;

			pictureBox1.SizeMode =  System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			pictureBox2.SizeMode =  System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			pictureBox3.SizeMode =  System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			pictureBox4.SizeMode =  System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			pictureBox5.SizeMode =  System.Windows.Forms.PictureBoxSizeMode.StretchImage;

			pictureBox1.Tag=1;
			pictureBox2.Tag=2;
			pictureBox3.Tag=3;
			pictureBox4.Tag=4;
			pictureBox5.Tag=5;
			pictureBox6.Tag=6;
			pictureBox7.Tag=7;
			pictureBox8.Tag=8;
			pictureBox9.Tag=9;

			//picNewsEvent1.Visible = false;

			UpdateDrawing();

			setImages();
			DrawSpeechThumbnails();
            setSpeechDateLabel(true);

		}

		// C#
		protected override void OnPaint(PaintEventArgs e) 
		{			
			// Redraw screen when form re-paints
			drawChartCanvas();
			DrawStockChart();
			DrawLegendKey();
			setNewsEvents();
		}

		// Draw blank chart canvas
		private void drawChartCanvas()
		{
			// Draw white canvas
			using (Graphics g = this.CreateGraphics())
			{
				// White background
				Color myColor;
				myColor = Color.White;
				g.FillRectangle(new SolidBrush(myColor),m_xAxisStart,m_yAxisEnd-80,m_ChartWidth,m_yAxisHeight+130);
				// Black 1pt border
				Pen myPen = new Pen(Color.Black,1);
				g.DrawRectangle(myPen,m_xAxisStart,m_yAxisEnd-80,m_ChartWidth,m_yAxisHeight+130);
			}
		}

		// Read stock data from text file.
		// Data originally from Yahoo Finance format
		// http://finance.yahoo.com/
		// Date,Open,High,Low,Close,Volume,Adj Close
		// Sample: 2008-10-31,43.00,44.82,42.10,43.25,3461700,43.25
		// =========================================================
		private void importData()
		{
			int fileIndex;
			string MyFileName = "";

			fileIndex = cboStockName.SelectedIndex;

            // Include sub-dir in path
            MyFileName = m_contentDir + "\\" + m_FileNames[fileIndex].ToString();

			FileStream aFile = new FileStream(MyFileName,FileMode.Open, FileAccess.Read);
			StreamReader sr = new StreamReader(aFile);		

			string strLine;

			// First line is stock name
			strLine = sr.ReadLine();	// read one line

			// Second line is column titles
			strLine = sr.ReadLine();	// read one line
			string[] strData;
			char[] chrDelimeter = new char[] {','};

			// Data starts on 3rd line
			strLine = sr.ReadLine();	// read one line

			// Read line from file and split into 8 dimensions
			while (strLine !=null)
			{
				strData = strLine.Split(chrDelimeter,10);

				m_array1.Add (strData[0]);
				m_array2.Add (strData[1]);
				m_array3.Add (strData[2]);
				m_array4.Add (strData[3]);
				m_array5.Add (strData[4]);
				m_array6.Add (strData[5]);
				m_array7.Add (strData[6]);
				
				strLine = sr.ReadLine();
			}
			sr.Close();			

			m_XYdata = new float[7,m_array1.Count];
			m_StockData = new StockInfo[m_array1.Count];

			for(int i=0;i<m_array1.Count;i++)
			{
				// Date,Open,High,Low,Close,Volume,Adj Close
				m_StockData[i].Date=System.Convert.ToString(m_array1[i]);	// Date
				m_StockData[i].PriceOpen=(System.Convert.ToSingle (m_array2[i]));	// Open
				m_StockData[i].PriceHigh=(System.Convert.ToSingle (m_array3[i]));	// High
				m_StockData[i].PriceLow=(System.Convert.ToSingle (m_array4[i]));	// Low
				m_StockData[i].PriceClose=(System.Convert.ToSingle (m_array5[i]));	// Close
				//m_StockData[i]=(System.Convert.ToSingle (m_array6[i]));	// Volume
				//m_StockData[i]=(System.Convert.ToSingle (m_array7[i]));	// Adj Close
			}

			// Convert data from string
			for(int i=0;i<m_array1.Count;i++)
			{
				// Date,Open,High,Low,Close,Volume,Adj Close
				//m_XYdata[0,i]=System.Convert.ToDateTime ( m_array1[i]);	// Date
				m_XYdata[1,i]=(System.Convert.ToSingle (m_array2[i]));	// Open
				m_XYdata[2,i]=(System.Convert.ToSingle (m_array3[i]));	// High
				m_XYdata[3,i]=(System.Convert.ToSingle (m_array4[i]));	// Low
				m_XYdata[4,i]=(System.Convert.ToSingle (m_array5[i]));	// Close
				m_XYdata[5,i]=(System.Convert.ToSingle (m_array6[i]));	// Volume
				m_XYdata[6,i]=(System.Convert.ToSingle (m_array7[i]));	// Adj Close
			}

			// Get min and max among stock price dimensions.
			// Date,Open,High,Low,Close,Volume,Adj Close
			m_yMin= getMinVal(3);
			m_yMax= getMaxVal(2);
		}

		// Draw chart for selected stock symbol and glyph type
		// ===================================
		private void DrawStockChart()
		{
			if (m_XYdata != null)
			{
				DrawYaxisLabels();

                // DrawXaxisLabels();

                //if(cboStockName.SelectedIndex == 0 )
                //{
                //    DrawXaxisLabels();
                //}

				if(chkShowMinMax.Checked)
				{
					DrawMinMaxLabels();
				}

				//adjust to coordinate system
				m_pAxisInterval = (m_ChartWidth / (m_StockData.GetUpperBound(0)+1));
                if (m_pAxisInterval <= 1)
                {
                    m_pAxisInterval = 2;
                }
                
                int xLoc=m_offsetX+(m_pAxisInterval/2);

				int valHigh, valClose, valOpen, valLow;

				// Step thru all data rows
				// NOTE: Must step BACKWARDS thru rows for left to right date progression
				// =========================================

				// Draw Line
				int y1,y2;
				if (rdoLine.Checked)
				{
					using (Graphics g = this.CreateGraphics())
					{
//						Image i = new Bitmap(m_ChartWidth,m_yAxisHeight);
//						Graphics ig = Graphics.FromImage(i);

						Pen myPen = new Pen(Color.Blue,1);
						GraphicsPath myPath = new GraphicsPath();
						for(int r=m_StockData.GetUpperBound(0);r>=1;r--)
						{
							// NOTE: Could change these equations to allow for different or custom
							// min max values for each axis so the data plots better vertically.
							//y1 = System.Convert.ToInt16((m_yAxisHeight)*(m_XYdata[c,i]/(m_yMaxs[c]-m_yMins[c])));
							//y1 = System.Convert.ToInt16((m_yAxisHeight)*((m_XYdata[c,i]-m_yMins[c])/(m_yMaxs[c]-m_yMins[c])));

							// Y1
							y1=System.Convert.ToInt16((m_yAxisHeight)*(m_StockData[r].PriceClose-m_yMin)/(1.1*(m_yMax-m_yMin)));
							//adjust to y offset
							y1 = m_yAxisStart  - y1;				

							y2=System.Convert.ToInt16((m_yAxisHeight)*(m_StockData[r-1].PriceClose-m_yMin)/(1.1*(m_yMax-m_yMin)));
							//adjust to y offset
							y2 = m_yAxisStart  - y2;				

							myPath.AddLine(xLoc,y1,xLoc+m_pAxisInterval,y2); 

							// draw x-axis tickmark
							myPen.Color = Color.Silver;
							g.DrawLine(myPen,xLoc,m_yAxisStart+44,xLoc,m_yAxisStart+49);

							xLoc=xLoc+m_pAxisInterval;
						}
						myPen.Color = Color.Blue;
						g.DrawPath(myPen,myPath);
						myPath.Dispose();
				
						//Rectangle myRec = new Rectangle(0,0,m_ChartWidth,m_yAxisHeight);
					//g.DrawImage(i, myRec);
						//i.Dispose;
					}
				}

				// For all other glyphs
				// Draw one of the glyph types, depending on user selection.
				//==========================================================
				for(int r=m_StockData.GetUpperBound(0);r>=0;r--)
					//for(int i=10; i<=200;i++)
				{
					// Step thru each column (dimension)
					using (Graphics g = this.CreateGraphics())
					{
						Pen myPen = new Pen(Color.Black,1);
						for(int c=1;c<=4;c++)
						{
							// Get daily price values.
							valOpen=System.Convert.ToInt16((m_yAxisHeight)*(m_StockData[r].PriceOpen-m_yMin)/(1.1*(m_yMax-m_yMin)));
							//adjust to y offset
							valOpen = m_yAxisStart  - valOpen;
							
							valClose=System.Convert.ToInt16((m_yAxisHeight)*(m_StockData[r].PriceClose-m_yMin)/(1.1*(m_yMax-m_yMin)));
							//adjust to y offset
							valClose = m_yAxisStart  - valClose;				

							valHigh= System.Convert.ToInt16((m_yAxisHeight)*(m_StockData[r].PriceHigh-m_yMin)/(1.1*(m_yMax-m_yMin)));
							//adjust to y offset
							valHigh = m_yAxisStart  - valHigh;
								
							valLow=System.Convert.ToInt16((m_yAxisHeight)*(m_StockData[r].PriceLow-m_yMin)/(1.1*(m_yMax-m_yMin)));
							//adjust to y offset
							valLow = m_yAxisStart  - valLow;

							Color myColor;

							// Set color for up or down day.
							if (valClose < valOpen)
							{
								myColor = Color.Green;
							}
							else if (valClose > valOpen)
							{
								myColor = Color.Red;
							}
							else
							{
								myColor = Color.Black;
							}
					
							// Draw high low glyph
							// ------------------------
							if (rdbHighLow.Checked)
							{
								myPen.Color = myColor;

								// Draw high low line
								g.DrawLine (myPen,xLoc,valLow,xLoc,valHigh);
							}

							// Draw OHLC - open, high, low, close glyph
							// ------------------------
							if (rdbOHLC.Checked)
							{
								myPen.Color = myColor;

								// Draw open and close tick
								g.DrawLine (myPen,xLoc-3,valOpen,xLoc,valOpen);
								g.DrawLine (myPen,xLoc,valClose,xLoc+3,valClose);								
								// Draw high low line
								g.DrawLine (myPen,xLoc,valLow,xLoc,valHigh);
							}

							// Draw Candlestick glyph
							// ------------------------
							if (rdbCandlestick.Checked)
							{
								int candleWidth = 3;

								// Set candlewidth based on number of data points
								if (m_StockData.GetUpperBound(0) >= 200)
								{
									candleWidth = 2;
								}
								
								if (m_StockData.GetUpperBound(0) < 200)
								{
									candleWidth = 5;
								}
								
								if (m_StockData.GetUpperBound(0) < 25)
								{
									candleWidth = 11;
								}																

								// Draw high low line
								g.DrawLine (myPen,xLoc,valLow,xLoc,valHigh);
							
								// Down day
								if(valOpen < valClose )
								{
									//myPen.Color = myColor;
									//g.FillRectangle(brush,x,y,w,h)
									g.FillRectangle(new SolidBrush(myColor),xLoc-((candleWidth/2)),valOpen,candleWidth+1,valClose-valOpen);

                                    // Draw border if not daily
                                    if(!(cboStockName.Text.Contains("Daily"))){
                                        myPen.Color = Color.Black;
                                        g.DrawRectangle(myPen, xLoc - (candleWidth / 2), valOpen, candleWidth, valClose - valOpen);
                                    }
								}

								// Up day
								else if(valOpen > valClose )
								{
									//myPen.Color = myColor;
									myPen.Color = Color.Black;
									//g.FillRectangle(new SolidBrush(Color.White),xLoc-2,valClose,candleWidth,valOpen-valClose);
                                    g.FillRectangle(new SolidBrush(myColor), xLoc - ((candleWidth / 2)), valClose, candleWidth + 1, valOpen - valClose);

                                    // Draw border if not daily
                                    if (!(cboStockName.Text.Contains("Daily")))
                                    {
                                        g.DrawRectangle(myPen, xLoc - (candleWidth / 2), valClose, candleWidth, valOpen - valClose);
                                    }
								}

								// No change day
								else if (valOpen == valClose )
								{
									myPen.Color = Color.Black;
									g.DrawLine(myPen,xLoc-(candleWidth/2),valOpen,xLoc+(candleWidth/2),valOpen);
								}
							}

							// Draw custom glyph
							// ------------------------
							if (rdbMyGlyph.Checked)
							{
								// g.DrawEllipse (myPen,x,y,w,h);
								g.DrawEllipse (myPen,xLoc-3,valOpen-3,6,6);
								g.FillEllipse (new SolidBrush(myColor),xLoc-4,valClose-4,8,8);
								g.DrawLine (myPen,xLoc-2,valHigh,xLoc+2,valHigh);
								g.DrawLine (myPen,xLoc-2,valLow,xLoc+2,valLow);
								g.DrawLine (myPen,xLoc,valLow,xLoc,valHigh);
							}

//							// Display Date string label for data value
//							StringFormat drawFormat = new StringFormat();
//							drawFormat.FormatFlags = StringFormatFlags.DirectionVertical;
//
//							Font myFnt = new Font("Verdana", 9);
							//g.DrawString(m_StockData[r].Date, myFnt, new SolidBrush(Color.Black),xLoc-5,m_yAxisStart+10,drawFormat);						
						}
						xLoc=xLoc+m_pAxisInterval;
					}
				}
			}
            DrawXaxisLabels();
            DrawLegendKey();
            
		}

		private void DrawYaxisLabels()
		{
			// Draw labels	
			using (Graphics g = this.CreateGraphics())
			{
				Font myFnt = new Font("Verdana", 7);

				// draw y-axis tickmark
				int yTic;
				Pen aPen = new Pen(Color.Blue,1);
				aPen.Color = Color.Black;

                int yLabelStart = 14;

                yTic = System.Convert.ToInt16((m_yAxisHeight) * (6000 - m_yMin) / (1.1 * (m_yMax - m_yMin)));
                yTic = m_yAxisStart - yTic;
                g.DrawLine(aPen, m_offsetX, yTic, m_offsetX + 5, yTic);
                g.DrawString("6000", myFnt, new SolidBrush(Color.Black), yLabelStart, yTic - 7);

                yTic = System.Convert.ToInt16((m_yAxisHeight) * (7000 - m_yMin) / (1.1 * (m_yMax - m_yMin)));
                yTic = m_yAxisStart - yTic;
                g.DrawLine(aPen, m_offsetX, yTic, m_offsetX + 5, yTic);
                g.DrawString("7000", myFnt, new SolidBrush(Color.Black), yLabelStart, yTic - 7);

				yTic=System.Convert.ToInt16((m_yAxisHeight)*(8000-m_yMin)/(1.1*(m_yMax-m_yMin)));
				yTic = m_yAxisStart  - yTic;				
				g.DrawLine(aPen,m_offsetX,yTic,m_offsetX+5,yTic);
                g.DrawString("8000", myFnt, new SolidBrush(Color.Black), yLabelStart, yTic - 7);

				yTic=System.Convert.ToInt16((m_yAxisHeight)*(9000-m_yMin)/(1.1*(m_yMax-m_yMin)));
				yTic = m_yAxisStart  - yTic;				
				g.DrawLine(aPen,m_offsetX,yTic,m_offsetX+5,yTic);
                g.DrawString("9000", myFnt, new SolidBrush(Color.Black), yLabelStart, yTic - 7);

				yTic=System.Convert.ToInt16((m_yAxisHeight)*(10000-m_yMin)/(1.1*(m_yMax-m_yMin)));
				yTic = m_yAxisStart  - yTic;				
				g.DrawLine(aPen,m_offsetX,yTic,m_offsetX+5,yTic);
                g.DrawString("10000", myFnt, new SolidBrush(Color.Black), yLabelStart-5, yTic - 7);

				yTic=System.Convert.ToInt16((m_yAxisHeight)*(11000-m_yMin)/(1.1*(m_yMax-m_yMin)));
				yTic = m_yAxisStart  - yTic;				
				g.DrawLine(aPen,m_offsetX,yTic,m_offsetX+5,yTic);
                g.DrawString("11000", myFnt, new SolidBrush(Color.Black), yLabelStart-5, yTic - 7);

				yTic=System.Convert.ToInt16((m_yAxisHeight)*(12000-m_yMin)/(1.1*(m_yMax-m_yMin)));
				yTic = m_yAxisStart  - yTic;				
				g.DrawLine(aPen,m_offsetX,yTic,m_offsetX+5,yTic);
                g.DrawString("12000", myFnt, new SolidBrush(Color.Black), yLabelStart-5, yTic - 7);

				yTic=System.Convert.ToInt16((m_yAxisHeight)*(13000-m_yMin)/(1.1*(m_yMax-m_yMin)));
				yTic = m_yAxisStart  - yTic;				
				g.DrawLine(aPen,m_offsetX,yTic,m_offsetX+5,yTic);
                g.DrawString("13000", myFnt, new SolidBrush(Color.Black), yLabelStart-5, yTic - 7);

				yTic=System.Convert.ToInt16((m_yAxisHeight)*(14000-m_yMin)/(1.1*(m_yMax-m_yMin)));
				yTic = m_yAxisStart  - yTic;				
				g.DrawLine(aPen,m_offsetX,yTic,m_offsetX+5,yTic);
                g.DrawString("14000", myFnt, new SolidBrush(Color.Black), yLabelStart-5, yTic - 7);
			}
		}


		// Draw month labels for x-axis
		private void DrawXaxisLabels()
		{
			int xLoc = m_offsetX;
			string strDate;
			DateTime myDate, prevDate;
			prevDate = DateTime.Now;

			for(int r=m_StockData.GetUpperBound(0);r>=0;r--)
			{	
				using (Graphics g = this.CreateGraphics())
				{
					myDate = System.Convert.ToDateTime(m_StockData[r].Date);
					
					//string strDate = myDate.Month +  ", " + myDate.Year;

                    // Display label for month
					if (myDate.Month != prevDate.Month)
					{
						//strDate= myDate.ToString("MMM, yyyy");
                        strDate = myDate.ToString("MMM");

						Font labelFnt = new Font("Verdana", 7);
						g.DrawString(strDate, labelFnt, new SolidBrush(Color.Black),xLoc-3,m_yAxisStart+52);

                        // Draw mark for the month
                        Pen myPen = new Pen(Color.Gray, 1);
                        g.DrawLine(myPen, xLoc, m_yAxisStart+42, xLoc, m_yAxisStart + 49);
					}

                    // Display label for year
                    if (myDate.Year != prevDate.Year)
                    {
                        //strDate= myDate.ToString("MMM, yyyy");
                        strDate = myDate.ToString("yyyy");

                        Font labelFnt = new Font("Verdana", 9, FontStyle.Bold);
                        g.DrawString(strDate, labelFnt, new SolidBrush(Color.Black), xLoc-3, m_yAxisStart + 64);
                    }
					prevDate = myDate;
				}
				xLoc = xLoc + m_pAxisInterval;
			}
		}

		private void DrawMinMaxLabels()
		{
			using (Graphics g = this.CreateGraphics())
			{			
				Font myFnt = new Font("Verdana", 8,FontStyle.Bold);
				int yMaxLabel;
				Pen yPen = new Pen(Color.Gray,1);

				yMaxLabel = System.Convert.ToInt16((m_yAxisHeight)*(m_yMax-m_yMin)/(1.1*(m_yMax-m_yMin)));
				yMaxLabel = m_yAxisStart  - yMaxLabel;

                
                string strMax = "Max:  " + System.Convert.ToString( m_yMax);
				g.DrawLine(yPen,m_offsetX+100,yMaxLabel,m_offsetX+220,yMaxLabel);
                g.DrawString(strMax, myFnt, new SolidBrush(Color.Black), 180, yMaxLabel - 16);

                string strMin = "Min:  " + System.Convert.ToString( m_yMin);
				g.DrawLine(yPen,m_offsetX+800,m_yAxisStart,m_offsetX+930,m_yAxisStart);
                g.DrawString(strMin, myFnt, new SolidBrush(Color.Black), 900, m_yAxisStart - 16);
			}

		}
		// Get min value for a given dimension.
		private float getMinVal(int c)
		{
			float myVal=0;
			if (m_XYdata != null)
			{
				myVal = m_XYdata[c,0];
				for(int i=1; i<=m_XYdata.GetUpperBound(1);i++)
				{
					if (m_XYdata[c,i] < myVal)
					{
						myVal = m_XYdata[c,i];
					}
				}
			}
			return myVal;
		}

		// Get max value for a given dimension.
		private float getMaxVal(int c)
		{
			float myVal=0;
			if (m_XYdata != null)
			{
				myVal = m_XYdata[c,0];
				for(int i=1; i<=m_XYdata.GetUpperBound(1);i++)
				{
					if (m_XYdata[c,i] > myVal)
					{
						myVal = m_XYdata[c,i];
					}
				}			
			}
			return myVal;
		}


		// Read list of CSV data files from program directory
		// =========================================
		private void getFileList()
		{
			// Create a reference to the content sub-directory
            string tmpDir = Environment.CurrentDirectory + "//" + m_contentDir;
            DirectoryInfo di = new DirectoryInfo(tmpDir);

            // Create an array representing the files in the content sub-directory
			FileInfo[] fi = di.GetFiles();

			foreach (FileInfo fiTemp in fi)
			{
				int cmpVal =(fiTemp.Extension.CompareTo(".csv"));
				if (cmpVal==0)
				{
                    // Get local path to read title from first line
                    string csvFileName = fiTemp.Directory.Name + "\\" + fiTemp.Name;
                    FileStream aFile = new FileStream(csvFileName, FileMode.Open, FileAccess.Read);
					StreamReader sr = new StreamReader(aFile);		
			
					string strLine;
			
					// First line is data name
					strLine = sr.ReadLine();	// read one line

					cboStockName.Items.Add(strLine);
					m_FileNames.Add(fiTemp.Name);
				}
			}
			cboStockName.SelectedIndex = 0;
		}


		private void setImages()
		{
			m_FedSpeeches = new FedSpeech[10];

			m_FedSpeeches[1].EventDay = 60;
			m_FedSpeeches[2].EventDay = 80;
			m_FedSpeeches[3].EventDay = 100;
			m_FedSpeeches[4].EventDay = 125;
			m_FedSpeeches[5].EventDay = 160;
			m_FedSpeeches[6].EventDay = 200;
			m_FedSpeeches[7].EventDay = 250;
			m_FedSpeeches[8].EventDay = 270;
			m_FedSpeeches[9].EventDay = 290;

		
			// Image files for each speech
			// 1-word random order
			// 1-word a-z order
			// 2-word a-z order
            m_FedSpeeches[1].ImageFile1 = m_contentDir + "\\" + "Federal Reserve Speech - Ben Bernanke - November 8, 2007-1r.PNG";
            m_FedSpeeches[2].ImageFile1 = m_contentDir + "\\" + "Federal Reserve Speech - Ben Bernanke - January 17, 2008-1r.PNG";
            m_FedSpeeches[3].ImageFile1 = m_contentDir + "\\" + "Federal Reserve Speech - Ben Bernanke - February 14, 2008-1r.PNG";
            m_FedSpeeches[4].ImageFile1 = m_contentDir + "\\" + "Federal Reserve Speech - Ben Bernanke - February 27, 2008-1r.PNG";
            m_FedSpeeches[5].ImageFile1 = m_contentDir + "\\" + "Federal Reserve Speech - Ben Bernanke - April 3, 2008-1r.PNG";
            m_FedSpeeches[6].ImageFile1 = m_contentDir + "\\" + "Federal Reserve Speech - Ben Bernanke - July 15, 2008-1r.PNG";
            m_FedSpeeches[7].ImageFile1 = m_contentDir + "\\" + "Federal Reserve Speech - Ben Bernanke - September 24, 2008-1r.PNG";
            m_FedSpeeches[8].ImageFile1 = m_contentDir + "\\" + "Federal Reserve Speech - Ben Bernanke - October 20, 2008-1r.PNG";
            m_FedSpeeches[9].ImageFile1 = m_contentDir + "\\" + "Federal Reserve Speech - Ben Bernanke - November 18, 2008-1r.PNG";

            m_FedSpeeches[1].ImageFile2 = m_contentDir + "\\" + "Federal Reserve Speech - Ben Bernanke - November 8, 2007-1a.PNG";
            m_FedSpeeches[2].ImageFile2 = m_contentDir + "\\" + "Federal Reserve Speech - Ben Bernanke - January 17, 2008-1a.PNG";
            m_FedSpeeches[3].ImageFile2 = m_contentDir + "\\" + "Federal Reserve Speech - Ben Bernanke - February 14, 2008-1a.PNG";
            m_FedSpeeches[4].ImageFile2 = m_contentDir + "\\" + "Federal Reserve Speech - Ben Bernanke - February 27, 2008-1a.PNG";
            m_FedSpeeches[5].ImageFile2 = m_contentDir + "\\" + "Federal Reserve Speech - Ben Bernanke - April 3, 2008-1a.PNG";
            m_FedSpeeches[6].ImageFile2 = m_contentDir + "\\" + "Federal Reserve Speech - Ben Bernanke - July 15, 2008-1a.PNG";
            m_FedSpeeches[7].ImageFile2 = m_contentDir + "\\" + "Federal Reserve Speech - Ben Bernanke - September 24, 2008-1a.PNG";
            m_FedSpeeches[8].ImageFile2 = m_contentDir + "\\" + "Federal Reserve Speech - Ben Bernanke - October 20, 2008-1a.PNG";
            m_FedSpeeches[9].ImageFile2 = m_contentDir + "\\" + "Federal Reserve Speech - Ben Bernanke - November 18, 2008-1a.PNG";

            m_FedSpeeches[1].ImageFile3 = m_contentDir + "\\" + "Federal Reserve Speech - Ben Bernanke - November 8, 2007-2a.PNG";
            m_FedSpeeches[2].ImageFile3 = m_contentDir + "\\" + "Federal Reserve Speech - Ben Bernanke - January 17, 2008-2a.PNG";
            m_FedSpeeches[3].ImageFile3 = m_contentDir + "\\" + "Federal Reserve Speech - Ben Bernanke - February 14, 2008-2a.PNG";
            m_FedSpeeches[4].ImageFile3 = m_contentDir + "\\" + "Federal Reserve Speech - Ben Bernanke - February 27, 2008-2a.PNG";
            m_FedSpeeches[5].ImageFile3 = m_contentDir + "\\" + "Federal Reserve Speech - Ben Bernanke - April 3, 2008-2a.PNG";
            m_FedSpeeches[6].ImageFile3 = m_contentDir + "\\" + "Federal Reserve Speech - Ben Bernanke - July 15, 2008-2a.PNG";
            m_FedSpeeches[7].ImageFile3 = m_contentDir + "\\" + "Federal Reserve Speech - Ben Bernanke - September 24, 2008-2a.PNG";
            m_FedSpeeches[8].ImageFile3 = m_contentDir + "\\" + "Federal Reserve Speech - Ben Bernanke - October 20, 2008-2a.PNG";
            m_FedSpeeches[9].ImageFile3 = m_contentDir + "\\" + "Federal Reserve Speech - Ben Bernanke - November 18, 2008-2a.PNG";

			// Assign thumbnail images
            m_myImages[0] = m_contentDir + "\\" + "test.png";	// placeholder
            m_myImages[1] = m_contentDir + "\\" + "thumbnail_November 8, 2007.PNG";
            m_myImages[2] = m_contentDir + "\\" + "thumbnail_January 17, 2008.PNG";
            m_myImages[3] = m_contentDir + "\\" + "thumbnail_February 14, 2008.PNG";
            m_myImages[4] = m_contentDir + "\\" + "thumbnail_February 27, 2008.PNG";
            m_myImages[5] = m_contentDir + "\\" + "thumbnail_April 3, 2008.PNG";
            m_myImages[6] = m_contentDir + "\\" + "thumbnail_July 15, 2008.PNG";
            m_myImages[7] = m_contentDir + "\\" + "thumbnail_September 24, 2008.PNG";
            m_myImages[8] = m_contentDir + "\\" + "thumbnail_October 20, 2008.PNG";
            m_myImages[9] = m_contentDir + "\\" + "thumbnail_November 18, 2008.PNG";
		}


		// Display news event images
		private void setNewsEvents()
		{
			if (chkShowNewsEvents.Checked)
			{
				m_NewsEvents = new NewsEvent[11];

				// Bear Stearns
                m_NewsEvents[1].ImageFile = m_contentDir + "\\" + "event_bear_stearns.jpg";
				m_NewsEvents[1].Title="";
				m_NewsEvents[1].Link="";           

				Bitmap myImage = new Bitmap(m_NewsEvents[1].ImageFile);
				picEventBearStearns.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
				picEventBearStearns.Image = (Image) myImage;
				picEventBearStearns.Visible = true;
				
				// Fannie Mae Freddie Mac
                m_NewsEvents[2].ImageFile = m_contentDir + "\\" + "event_fannie_freddie.jpg";
				m_NewsEvents[2].Title="";
				m_NewsEvents[2].Link="";           

				myImage = new Bitmap(m_NewsEvents[2].ImageFile);
				picEventFannieFreddie.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
				picEventFannieFreddie.Image = (Image) myImage;
				picEventFannieFreddie.Visible = true;

				// Lehman Brothers
                m_NewsEvents[3].ImageFile = m_contentDir + "\\" + "event_lehman_brothers.jpg";
				m_NewsEvents[3].Title="";
				m_NewsEvents[3].Link="";           

				myImage = new Bitmap(m_NewsEvents[3].ImageFile);
				picEventLehmanBros.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
				picEventLehmanBros.Image = (Image) myImage;
				picEventLehmanBros.Visible = true;

				// AIG
                m_NewsEvents[4].ImageFile = m_contentDir + "\\" + "event_aig_logo.jpg";
				m_NewsEvents[4].Title="";
				m_NewsEvents[4].Link="";           

				myImage = new Bitmap(m_NewsEvents[4].ImageFile);
				picEventAIG.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
				picEventAIG.Image = (Image) myImage;
				picEventAIG.Visible = true;

                // IndyMac Bank - placeholder
			}
			// Hide images
			else
			{
				picEventBearStearns.Visible = false;
				picEventFannieFreddie.Visible = false;
				picEventLehmanBros.Visible = false;
				picEventAIG.Visible = false;
			}

		}

		private void drawLineToEvent(int eventID)
		{
			int EventLocX;
			Form1.ActiveForm.Refresh(); 
			//			DrawStockChart();

			// start line
			int y = grpSpeech.Location.Y;  
			int x = grpSpeech.Location.X + (grpSpeech.Width/2);

            //EventLocX = m_FedSpeeches[eventID].EventDay;
            //EventLocX = EventLocX * m_pAxisInterval;

            switch (eventID)
            {
                case 1:
                    EventLocX = pictureBox1.Left + pictureBox1.Width / 2;
                    break;

                case 2:
                    EventLocX = pictureBox2.Left + pictureBox2.Width / 2;
                    break;

                case 3:
                    EventLocX = pictureBox3.Left + pictureBox3.Width / 2;
                    break;

                case 4:
                    EventLocX = pictureBox4.Left + pictureBox4.Width / 2;
                    break;

                case 5:
                    EventLocX = pictureBox5.Left + pictureBox5.Width / 2;
                    break;

                case 6:
                    EventLocX = pictureBox6.Left + pictureBox6.Width / 2;
                    break;

                case 7:
                    EventLocX = pictureBox7.Left + pictureBox7.Width / 2;
                    break;

                case 8:
                    EventLocX = pictureBox8.Left + pictureBox8.Width / 2;
                    break;

                case 9:
                    EventLocX = pictureBox9.Left + pictureBox9.Width / 2;
                    break;

                default:
                    EventLocX = pictureBox1.Left + pictureBox1.Width / 2;
                    break;
            }

			using (Graphics g = this.CreateGraphics())
			{

				Pen myPen = new Pen(Color.Black,1);

				//g.DrawLine (myPen,x,y,EventLocX,m_yAxisEnd+25);

				// Create points that define polygon.
				Point point1 = new Point( x, y);
				Point point2 = new Point(x+50,y);
                Point point3 = new Point(EventLocX, pictureBox1.Bottom);
				Point[] myPoints ={	point1,	point2,	point3};

                // Make semitransparent color
                var semiLightBlue = Color.FromArgb(125, Color.LightBlue);

                g.FillPolygon(new SolidBrush(semiLightBlue), myPoints);

			}
		}

		// Test mouse click
		private void pictureBox1_Click(object sender, System.EventArgs e)
		{				
			restPicBoxBorder();
			pictureBox1.BorderStyle =  System.Windows.Forms.BorderStyle.Fixed3D;

            //pictureBox1.BackColor = System.Drawing.Color.LightBlue;
            //this.pictureBox1.Padding = new System.Windows.Forms.Padding(3);
			
			m_speechID = System.Convert.ToInt16(pictureBox1.Tag); 

			DisplaySelectedSpeech(m_speechID);
			drawLineToEvent(m_speechID);
		}

		// Test mouse hover
		private void pictureBox5_MouseHover(object sender, System.EventArgs e)
		{
			// pictureBox5.BorderStyle =  System.Windows.Forms.BorderStyle.Fixed3D;
			//			pictureBox5.Width = 200;
			//			pictureBox5.Height = 200;
		}

		private void pictureBox5_Click(object sender, System.EventArgs e)
		{
			restPicBoxBorder();
			pictureBox5.BorderStyle =  System.Windows.Forms.BorderStyle.Fixed3D;

			
			m_speechID = System.Convert.ToInt16(pictureBox5.Tag); 

			DisplaySelectedSpeech(m_speechID);
			drawLineToEvent(m_speechID);
		}

		private void pictureBox4_MouseLeave(object sender, System.EventArgs e)
		{
		
			// pictureBox4.BorderStyle =  System.Windows.Forms.BorderStyle.None;
			//			pictureBox4.Width = 100;
			//			pictureBox4.Height = 100;

		}

		private void pictureBox4_MouseEnter(object sender, System.EventArgs e)
		{
			// pictureBox4.BorderStyle =  System.Windows.Forms.BorderStyle.FixedSingle;
			//			pictureBox4.Width = 200;
			//			pictureBox4.Height = 200;
		
		}

		private void pictureBox3_Click(object sender, System.EventArgs e)
		{
			restPicBoxBorder();
			pictureBox3.BorderStyle =  System.Windows.Forms.BorderStyle.Fixed3D;

			
			m_speechID = System.Convert.ToInt16(pictureBox3.Tag); 

			DisplaySelectedSpeech(m_speechID);
			drawLineToEvent(m_speechID);
		}


		private void pictureBox2_Click(object sender, System.EventArgs e)
		{
			restPicBoxBorder();
			pictureBox2.BorderStyle =  System.Windows.Forms.BorderStyle.Fixed3D;

			
			m_speechID = System.Convert.ToInt16(pictureBox2.Tag); 

			DisplaySelectedSpeech(m_speechID);
			drawLineToEvent(m_speechID);
		}

		private void DrawSpeechThumbnails()
		{
			string myFile = m_contentDir + "\\" + "test.png";
			Bitmap myImage = new Bitmap(myFile);

			// Draw thumbnails
            myFile = m_myImages[1];
			myImage = new Bitmap(myFile);
			pictureBox1.Image = (Image) myImage;

            myFile = m_myImages[2];
			myImage = new Bitmap(myFile);
			pictureBox2.Image = (Image) myImage;

            myFile = m_myImages[3];
			myImage = new Bitmap(myFile);
			pictureBox3.Image = (Image) myImage;

            myFile = m_myImages[4];
			myImage = new Bitmap(myFile);
			pictureBox4.Image = (Image) myImage;

            myFile = m_myImages[5];
			myImage = new Bitmap(myFile);
			pictureBox5.Image = (Image) myImage;

            myFile = m_myImages[6];
			myImage = new Bitmap(myFile);
			pictureBox6.Image = (Image) myImage;

            myFile =  m_myImages[7];
			myImage = new Bitmap(myFile);
			pictureBox7.Image = (Image) myImage;

            myFile = m_myImages[8];
			myImage = new Bitmap(myFile);
			pictureBox8.Image = (Image) myImage;

            myFile = m_myImages[9];
			myImage = new Bitmap(myFile);
			pictureBox9.Image = (Image) myImage;
		
			//			drawLineToEvent(n);
		}

        private void setSpeechDateLabel(Boolean showText)
        {
            if (showText == true) {
                lblSpeechDate1.Text = "11/2007";
                lblSpeechDate2.Text = "12/2007";
                lblSpeechDate3.Text = "1/2008";
                lblSpeechDate4.Text = "2/2008";
                lblSpeechDate5.Text = "4/2008";
                lblSpeechDate6.Text = "7/2008";
                lblSpeechDate7.Text = "9/2008";
                lblSpeechDate8.Text = "10/2008";
                lblSpeechDate9.Text = "11/2008";
            }
            else {
                lblSpeechDate1.Text = "";
                lblSpeechDate2.Text = "";
                lblSpeechDate3.Text = "";
                lblSpeechDate4.Text = "";
                lblSpeechDate5.Text = "";
                lblSpeechDate6.Text = "";
                lblSpeechDate7.Text = "";
                lblSpeechDate8.Text = "";
                lblSpeechDate9.Text = "";
            }
        }

		private void restPicBoxBorder()
		{
			pictureBox1.BorderStyle =  System.Windows.Forms.BorderStyle.FixedSingle;
			pictureBox2.BorderStyle =  System.Windows.Forms.BorderStyle.FixedSingle;
			pictureBox3.BorderStyle =  System.Windows.Forms.BorderStyle.FixedSingle;
			pictureBox4.BorderStyle =  System.Windows.Forms.BorderStyle.FixedSingle;
			pictureBox5.BorderStyle =  System.Windows.Forms.BorderStyle.FixedSingle;
			pictureBox6.BorderStyle =  System.Windows.Forms.BorderStyle.FixedSingle;
			pictureBox7.BorderStyle =  System.Windows.Forms.BorderStyle.FixedSingle;
			pictureBox8.BorderStyle =  System.Windows.Forms.BorderStyle.FixedSingle;
			pictureBox9.BorderStyle =  System.Windows.Forms.BorderStyle.FixedSingle;
		}

		private void pictureBox4_Click(object sender, System.EventArgs e)
		{
			restPicBoxBorder();
			pictureBox4.BorderStyle =  System.Windows.Forms.BorderStyle.Fixed3D;

			
			m_speechID = System.Convert.ToInt16(pictureBox4.Tag); 

			DisplaySelectedSpeech(m_speechID);
			drawLineToEvent(m_speechID);
		}

		private void pictureBox6_Click(object sender, System.EventArgs e)
		{
			restPicBoxBorder();
			pictureBox6.BorderStyle =  System.Windows.Forms.BorderStyle.Fixed3D;

			
			m_speechID = System.Convert.ToInt16(pictureBox6.Tag); 

			DisplaySelectedSpeech(m_speechID);
			drawLineToEvent(m_speechID);
		}

		private void pictureBox7_Click(object sender, System.EventArgs e)
		{
			restPicBoxBorder();
			pictureBox7.BorderStyle =  System.Windows.Forms.BorderStyle.Fixed3D;
			
			m_speechID = System.Convert.ToInt16(pictureBox7.Tag); 

			DisplaySelectedSpeech(m_speechID);
			drawLineToEvent(m_speechID);
		}

		private void pictureBox8_Click(object sender, System.EventArgs e)
		{
			restPicBoxBorder();
			pictureBox8.BorderStyle =  System.Windows.Forms.BorderStyle.Fixed3D;

			
			m_speechID = System.Convert.ToInt16(pictureBox8.Tag); 

			DisplaySelectedSpeech(m_speechID);
			drawLineToEvent(m_speechID);
		}

		private void pictureBox9_Click(object sender, System.EventArgs e)
		{
			restPicBoxBorder();
			pictureBox9.BorderStyle =  System.Windows.Forms.BorderStyle.Fixed3D;
			
			m_speechID = System.Convert.ToInt16(pictureBox9.Tag); 

			DisplaySelectedSpeech(m_speechID);
			drawLineToEvent(m_speechID);
		}

		// STOCK GLYPH LEGEND - Draw sample of each glyph
		//=============================
		private void DrawLegendKey()
		{
			int valHigh, valClose, valOpen, valLow;
			valOpen = 4;						
			valClose = 8;				
			valHigh = 12;					
			valLow = 2;

			int yStartLoc = 2;
			int xLoc = 4;

            // LEGEND - Draw line chart
            // --------------------------
			using (Graphics g = picLegendLine.CreateGraphics())
			{
				Pen myPen = new Pen(Color.Blue,1);
				g.DrawLine (myPen,xLoc,yStartLoc,xLoc+10,yStartLoc+8);
				g.DrawLine (myPen,xLoc+10,yStartLoc+8,xLoc+20,yStartLoc+4);
                g.DrawLine(myPen, xLoc + 20, yStartLoc+4, xLoc + 30, yStartLoc+12);
			}

            // LEGEND - Draw High-Low glyph
            // --------------------------
			using (Graphics g = picLegendHighLow.CreateGraphics())
			{
				Pen myPen = new Pen(Color.Black,1);
				valLow = 17;
				valHigh = 5;
                xLoc = 5;

                // Up day
				myPen.Color = Color.Green;
				g.DrawLine (myPen,xLoc,valLow,xLoc,valHigh);

                // Down day
				myPen.Color = Color.Red;
				g.DrawLine (myPen,xLoc+5,valLow+5,xLoc+5,valHigh+3);

                // Up day
				myPen.Color = Color.Green;
				g.DrawLine (myPen,xLoc+10,valLow-2,xLoc+10,valHigh-2);
			}

            // LEGEND - Draw Open,High,Low,Close (OHLC) glyph
            // --------------------------
			using (Graphics g = picLegendOHLC.CreateGraphics())
			{
				Pen myPen = new Pen(Color.Red,1);
				
                // Draw Down day
                valOpen = 11;
                valClose = 18;
                valHigh = 25;
                valLow = 9;
                xLoc = 10;
                // Draw open and close tick
				g.DrawLine (myPen,xLoc-3,valOpen,xLoc,valOpen);
				g.DrawLine (myPen,xLoc,valClose,xLoc+3,valClose);								
				// Draw high low line
				g.DrawLine (myPen,xLoc,valLow,xLoc,valHigh);

                // Draw Up day
                valOpen = 15;
                valClose = 7;
                valHigh = 20;
                valLow = 2;
                xLoc = 20;
                myPen.Color = Color.Green;

                // Draw open and close tick
                g.DrawLine(myPen, xLoc - 3, valOpen, xLoc, valOpen);
                g.DrawLine(myPen, xLoc, valClose, xLoc + 3, valClose);
                // Draw high low line
                g.DrawLine(myPen, xLoc, valLow, xLoc, valHigh);

			}

            // LEGEND - Draw Candlestick glyph
            // ------------------------
			using (Graphics g = picLegendCandlestick.CreateGraphics())
			{
				Pen myPen = new Pen(Color.Black,1);
				int candleWidth = 4;

                // Draw a sample Down day
                valOpen = 12;
                valClose = 18;
                valHigh = 25;
                valLow = 6;
                xLoc = 10;

				// Draw high low line
				g.DrawLine (myPen,xLoc,valLow,xLoc,valHigh);

                //g.FillRectangle(brush,x,y,w,h)
				myPen.Color = Color.Red;
				g.FillRectangle(new SolidBrush(Color.Red),xLoc-((candleWidth/2)),valOpen,candleWidth+1,valClose-valOpen);

				// Draw a sample Up day
                valOpen = 7;
                valClose = 17;
                valHigh = 22;
                valLow = 2;
                xLoc =20;

                // Draw high low line
                myPen.Color = Color.Black;
                g.DrawLine(myPen, xLoc, valLow, xLoc, valHigh);

				myPen.Color = Color.Black;
                g.FillRectangle(new SolidBrush(Color.Green), xLoc - ((candleWidth / 2)), valOpen, candleWidth + 1, valClose - valOpen);
			}

            // LEGEND - Draw custom glyph
            // ------------------------
            using (Graphics g = picCustomGlyph.CreateGraphics())
            {
                Pen myPen = new Pen(Color.Black, 1);
                // Draw a sample Down day
                valOpen = 15;
                valClose = 27;
                valHigh = 39;
                valLow = 9;
                xLoc = 10;

                // g.DrawEllipse (myPen,x,y,w,h);
                g.DrawEllipse(myPen, xLoc - 3, valOpen - 3, 6, 6);
                g.FillEllipse(new SolidBrush(Color.Red), xLoc - 4, valClose - 4, 8, 8);
                g.DrawLine(myPen, xLoc - 2, valHigh, xLoc + 2, valHigh);
                g.DrawLine(myPen, xLoc - 2, valLow, xLoc + 2, valLow);
                g.DrawLine(myPen, xLoc, valLow, xLoc, valHigh);

                // Draw a sample Up day
                valOpen = 17;
                valClose = 7;
                valHigh = 27;
                valLow = 2;
                xLoc = 23;

                g.DrawEllipse(myPen, xLoc - 3, valOpen - 3, 6, 6);
                g.FillEllipse(new SolidBrush(Color.Green), xLoc - 4, valClose - 4, 8, 8);
                g.DrawLine(myPen, xLoc - 2, valHigh, xLoc + 2, valHigh);
                g.DrawLine(myPen, xLoc - 2, valLow, xLoc + 2, valLow);
                g.DrawLine(myPen, xLoc, valLow, xLoc, valHigh);
            }

            // Update each image
            picLegendLine.Update();
            picLegendHighLow.Update();
            picLegendOHLC.Update();
            picLegendCandlestick.Update();
            picCustomGlyph.Update();
		}

		private void rdbHighLow_CheckedChanged(object sender, System.EventArgs e)
		{
			Form1.ActiveForm.Refresh();
		}

		private void rdbOHLC_CheckedChanged(object sender, System.EventArgs e)
		{
			Form1.ActiveForm.Refresh();
		}

		private void rdbCandlestick_CheckedChanged(object sender, System.EventArgs e)
		{
			Form1.ActiveForm.Refresh();
		}

		private void rdbMyGlyph_CheckedChanged(object sender, System.EventArgs e)
		{
			Form1.ActiveForm.Refresh();
		}

		private void cboStockName_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			UpdateDrawing();
            
			Form1.ActiveForm.Refresh();
            //DrawXaxisLabels();
		}

		private void groupBox1_Enter(object sender, System.EventArgs e)
		{
		
		}

		private void groupBox2_Enter(object sender, System.EventArgs e)
		{
		
		}

		private void chkShowMinMax_CheckedChanged(object sender, System.EventArgs e)
		{

		}

		private void groupBox5_Enter(object sender, System.EventArgs e)
		{
		
		}

		private void chkShowNewsEvents_CheckedChanged(object sender, System.EventArgs e)
		{
			Form1.ActiveForm.Refresh(); 
		}

		private void checkBox3_CheckedChanged(object sender, System.EventArgs e)
		{
			Form1.ActiveForm.Refresh();
		}

		private void UpdateDrawing()
		{
			//Form1.ActiveForm.Refresh();  
			// clear data
			m_array1.Clear();
			m_array2.Clear();
			m_array3.Clear();
			m_array4.Clear();
			m_array5.Clear();
			m_array6.Clear();
			m_array7.Clear();
			
			// Import data
			importData();
		}


		// Display selected images
		private void DisplaySelectedSpeech(int x)
		{
			string myFile;
			Bitmap myImage;

			switch (x)
			{
				case 1:
					picSpeech1.Image = null;

                    myFile = m_FedSpeeches[1].ImageFile1;
					myImage = new Bitmap(myFile);

					picSpeech1.Image = (Image) myImage;

                    myFile = m_FedSpeeches[1].ImageFile3;
					myImage = new Bitmap(myFile);
					picSpeech2.Image = (Image) myImage;
					break;                  

				case 2:
					picSpeech1.Image = null;

                    myFile = m_FedSpeeches[2].ImageFile1;
					myImage = new Bitmap(myFile);

					picSpeech1.Image = (Image) myImage;

                    myFile =  m_FedSpeeches[2].ImageFile3;
					myImage = new Bitmap(myFile);
					picSpeech2.Image = (Image) myImage;
					break;                  

				case 3:
					picSpeech1.Image = null;

                    myFile = m_FedSpeeches[3].ImageFile1;
					myImage = new Bitmap(myFile);

					picSpeech1.Image = (Image) myImage;

                    myFile = m_FedSpeeches[3].ImageFile3;
					myImage = new Bitmap(myFile);
					picSpeech2.Image = (Image) myImage;
					break;                  

				case 4:
					picSpeech1.Image = null;

                    myFile = m_FedSpeeches[4].ImageFile1;
					myImage = new Bitmap(myFile);

					picSpeech1.Image = (Image) myImage;

                    myFile = m_FedSpeeches[4].ImageFile3;
					myImage = new Bitmap(myFile);
					picSpeech2.Image = (Image) myImage;
					break;                  

				case 5:
					picSpeech1.Image = null;

                    myFile = m_FedSpeeches[5].ImageFile1;
					myImage = new Bitmap(myFile);

					picSpeech1.Image = (Image) myImage;

                    myFile = m_FedSpeeches[5].ImageFile3;
					myImage = new Bitmap(myFile);
					picSpeech2.Image = (Image) myImage;
					break;                  

				case 6:
					picSpeech1.Image = null;

                    myFile = m_FedSpeeches[6].ImageFile1;
					myImage = new Bitmap(myFile);

					picSpeech1.Image = (Image) myImage;

                    myFile = m_FedSpeeches[6].ImageFile3;
					myImage = new Bitmap(myFile);
					picSpeech2.Image = (Image) myImage;
					break;                  

				case 7:
					picSpeech1.Image = null;

                    myFile = m_FedSpeeches[7].ImageFile1;
					myImage = new Bitmap(myFile);

					picSpeech1.Image = (Image) myImage;

                    myFile = m_FedSpeeches[7].ImageFile3;
					myImage = new Bitmap(myFile);
					picSpeech2.Image = (Image) myImage;
					break;                  

				case 8:
					picSpeech1.Image = null;

                    myFile = m_FedSpeeches[8].ImageFile1;
					myImage = new Bitmap(myFile);

					picSpeech1.Image = (Image) myImage;

                    myFile = m_FedSpeeches[8].ImageFile3;
					myImage = new Bitmap(myFile);
					picSpeech2.Image = (Image) myImage;
					break;                  

				case 9:
					picSpeech1.Image = null;

                    myFile = m_FedSpeeches[9].ImageFile1;
					myImage = new Bitmap(myFile);

					picSpeech1.Image = (Image) myImage;

                    myFile = m_FedSpeeches[9].ImageFile3;
					myImage = new Bitmap(myFile);
					picSpeech2.Image = (Image) myImage;
					break;                  

				default:            
					break;
			}
		}

		private void rdoLine_CheckedChanged(object sender, System.EventArgs e)
		{
			Form1.ActiveForm.Refresh(); 
		}

		private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			string[] imageFiles = new string[4];

			imageFiles[0]="";
			imageFiles[1]=m_FedSpeeches[m_speechID].ImageFile1;
			imageFiles[2]=m_FedSpeeches[m_speechID].ImageFile2;
			imageFiles[3]=m_FedSpeeches[m_speechID].ImageFile3;

			Form2 frm = new Form2();
			frm.Show();
			frm.DisplayImages(imageFiles);
		}

        private void drawIntRates (){

        // ------------------------------------------

            //picIntRates

            using (Graphics g = this.CreateGraphics())
            {
                Pen myPen = new Pen(Color.Red, 2);
                //GraphicsPath myPath = new GraphicsPath();
                //myPath.AddLine(xLoc,y1,xLoc+m_pAxisInterval,y2); 

                // draw x-axis tickmark
                g.DrawLine(myPen, 10, 10, 20, 20);

                //xLoc=xLoc+m_pAxisInterval;
                //    }
                //    myPen.Color = Color.Blue;
                //    g.DrawPath(myPen,myPath);
                //    myPath.Dispose();
            }
        }
    // ------------------------------------------





        private void checkBox3_CheckedChanged_1(object sender, EventArgs e)
        {
            if (lblSpeechDate1.Text == "")
            {
                setSpeechDateLabel(true);
            }
            else{
                setSpeechDateLabel(false);
            }


            if (pictureBox1.Image != null)
            {
                pictureBox1.Image = null;
                pictureBox2.Image = null;
                pictureBox3.Image = null;
                pictureBox4.Image = null;
                pictureBox5.Image = null;
                pictureBox6.Image = null;
                pictureBox7.Image = null;
                pictureBox8.Image = null;
                pictureBox9.Image = null;

                pictureBox1.BorderStyle = BorderStyle.None;
                pictureBox2.BorderStyle = BorderStyle.None;
                pictureBox3.BorderStyle = BorderStyle.None;
                pictureBox4.BorderStyle = BorderStyle.None;
                pictureBox5.BorderStyle = BorderStyle.None;
                pictureBox6.BorderStyle = BorderStyle.None;
                pictureBox7.BorderStyle = BorderStyle.None;
                pictureBox8.BorderStyle = BorderStyle.None;
                pictureBox9.BorderStyle = BorderStyle.None;
            }
            else
            {
                DrawSpeechThumbnails();

                pictureBox1.BorderStyle = BorderStyle.FixedSingle;
                pictureBox2.BorderStyle = BorderStyle.FixedSingle;
                pictureBox3.BorderStyle = BorderStyle.FixedSingle;
                pictureBox4.BorderStyle = BorderStyle.FixedSingle;
                pictureBox5.BorderStyle = BorderStyle.FixedSingle;
                pictureBox6.BorderStyle = BorderStyle.FixedSingle;
                pictureBox7.BorderStyle = BorderStyle.FixedSingle;
                pictureBox8.BorderStyle = BorderStyle.FixedSingle;
                pictureBox9.BorderStyle = BorderStyle.FixedSingle;

            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }


	}
}
