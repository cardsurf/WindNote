using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WindNote.Gui.CustomControls;

namespace WindNote.Gui.UserControls
{

    public partial class DragableRowHeader : UserControl
    {

        public DragableRowHeader()
        {
            InitializeComponent();
            this.AddMouseMoveHanlder();
        }

        private void AddMouseMoveHanlder()
        {
            DragableRowHeaderMouseMoveHandler handler = new DragableRowHeaderMouseMoveHandler();
            this.MouseMove += handler.StartDragAndDrop;
        }

    }
}
