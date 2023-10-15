using System;
using System.Drawing;
using System.Windows.Forms;

public class Rectangle : Form
{
    public Rectangle()
    {
        this.Size = new Size(200, 200);
    }
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        Graphics g = e.Graphics;
        int x = 10;       
        int y = 10;        
        int width = 150;   
        int height = 100;  
        Pen pen = new Pen(Color.Aquamarine, 2); 
        g.DrawRectangle(pen, x, y, width, height);
        pen.Dispose();
    }
    public static void Main()
    {
        Application.Run(new Rectangle());
    }
}
