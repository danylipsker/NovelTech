using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace NovelTech.viewmodels.PolygonCreator
{
    public class EdgeViewModel
    {
        #region Properties
        private VertexViewModel p1;

        public VertexViewModel P1
        {
            get { return p1; }
            set { p1 = value; }
        }

        private VertexViewModel p2;

        public VertexViewModel P2
        {
            get { return p2; }
            set { p2 = value; }
        }
        #endregion

        #region Fields
        private PolygonCreatorViewModel mainVM;
        #endregion

        #region Constructors
        public EdgeViewModel(PolygonCreatorViewModel mainVM)
        {
            this.mainVM = mainVM;
        }
        #endregion

        #region Methods

        public void AddEdge(Point point)
        {
            mainVM.AddVertex(this, point);
        }
        #endregion
    }
}
