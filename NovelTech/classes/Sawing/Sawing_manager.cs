using MethodsLibrary;
using NovelTech.libraries;
using NovelTech.viewmodels;
using NovelTech.views.usercontrols;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace NovelTech.classes.Sawing
{
    public class Sawing_manager
    {
        #region Vars
        #region UserControls
        //private UC_machine_table host; // the frame that host the animation
        private UC_shape leader; // the control that upon him we will do the calculations
        private UC_material material; // the controls that will be grouped to the animations
        //private UC_pincher pincher; // the controls that will be grouped to the animations
        private UC_tool saw; // the tool
        private UC_tool drill; // the tool
        #endregion

        #region StoryBoard
        private Storyboard storyboard = new Storyboard();
        private DoubleAnimationUsingKeyFrames material_angle = new DoubleAnimationUsingKeyFrames();
        private ThicknessAnimationUsingKeyFrames material_margin = new ThicknessAnimationUsingKeyFrames();

        private float target_time = 0;
        private float drill_time = 5;
        private int count_finished = 0;
        private int req_finished = 2;
        private float speed;
        #endregion

        #region Caluculating
        private Point blade_absolute;
        private Point drill_absolute;
        public double[] angles;
        public PointCollection drillPoints;
        private string curr_action = "rotate";
        private int curr_index = 0;
        private double raw_material_angle;
        int counterI = 0;
        #endregion

        #region GCode
        private List<Saw_step> steps = new List<Saw_step>();
        public string[] gcodes;
        #endregion

        #endregion

        #region Constructor
        public Sawing_manager(double[] angles, PointCollection drillPoints, UC_machine_table host,
            UC_shape leader, UC_material material, UC_tool saw, UC_tool drill)
        {
            counterI = 0;
            this.drillPoints = drillPoints;
            this.angles = angles;
            //this.host = host;
            this.leader = leader;
            this.material = material;
            //this.pincher = pincher;
            this.saw = saw;
            this.drill = drill;
            this.speed = 2*(1/ float.Parse(UC_machine_table.instance.Feedrate.Text));//bigger feedrate should mean smaller waiting time temporry equition subject to change
            raw_material_angle = ((material.RenderTransform as TransformGroup).Children[2] as RotateTransform).Angle;
            sawing_start();
        }
        #endregion

        #region Methods
        #region Pre Animation
        private void sawing_start()
        {
            blade_absolute = points.get_absolute(saw.blade, Application.Current.MainWindow);

            drill_absolute = points.get_absolute(drill.blade, Application.Current.MainWindow);

            saw.ColorElement.Background = Brushes.Green;
            drill.ColorElement.Background = Brushes.Red;

            bind_animations();

            saw_step_manage();
        }

        private void bind_animations()
        {
            material_angle.Completed += animation_step_completed;
            material_margin.Completed += animation_step_completed;

            storyboard.Children.Add(material_angle);
            storyboard.Children.Add(material_margin);

            Storyboard.SetTarget(material_angle, material);
            Storyboard.SetTarget(material_margin, material);

            Storyboard.SetTargetProperty(material_angle, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[2].(RotateTransform.Angle)"));
            Storyboard.SetTargetProperty(material_margin, new PropertyPath(FrameworkElement.MarginProperty));

        }
        #endregion


        void switch_to_drill()
        {
            saw.ColorElement.Background = Brushes.Red;
            drill.ColorElement.Background = Brushes.Green;

            UC_machine_table.instance.lToolIndex = 1;
            UC_machine_table.instance.UpdateRPMFeedrateValues();


            this.speed = 2 * (1 / float.Parse(UC_machine_table.instance.Feedrate.Text));//bigger feedrate should mean smaller waiting time temporry equition subject to change
        }

        #region Animating
        private void saw_step_manage()
        {
            double x_move, y_move;
            count_finished = 0;
            set_animations_time();
            Vector lower;

            if (curr_action!= "drill_move_x" && curr_action!= "drill_move_y"&& curr_action != "drill_move_y_by_margin" && curr_action != "drill_move_x_by_margin" && curr_action != "drill_rotate")
            {
                #region get the points of the line that is being cut
                Point p1, p2;
                p1 = PolygonMethods.GetPointAbsolute(VM_shape.instance.polygon, curr_index, Application.Current.MainWindow);
                if (curr_index < angles.Length - 1)
                {
                    p2 = PolygonMethods.GetPointAbsolute(VM_shape.instance.polygon, curr_index + 1, Application.Current.MainWindow);

                }
                else
                {
                    p2 = PolygonMethods.GetPointAbsolute(VM_shape.instance.polygon, 0, Application.Current.MainWindow);
                }
                p1.Y = Application.Current.MainWindow.ActualHeight - p1.Y;
                p2.Y = Application.Current.MainWindow.ActualHeight - p2.Y;
                lower = VectorMethods.GetLower(p1, p2);
                #endregion
            }
            else
            {
                #region get cur drill point
                if (counterI < drillPoints.Count)
                {
                    UC_machine_table.instance.Depth.Text = VM_shape.instance.drillPointsDepths[counterI].ToString();
                    Point temp = points.get_absolute(UC_shape.instance, Application.Current.MainWindow, false);
                    //10 becuese it's half the size of the ellipse that is the drill point
                    lower = (Vector)Point.Subtract(temp, new Point(-drillPoints[counterI].X - 10 + (drill.blade.Width / 2), drillPoints[counterI].Y + 10 + (drill.blade.Height / 2))); ;

                }
                #endregion
            }
            switch (curr_action)
            {
                #region saw
                case "rotate":
                    set_rotate_step(angles[curr_index] + raw_material_angle);
                    break;
                case "rotate_opposite":
                    var angle = angles[curr_index] + raw_material_angle;
                    if (angle >= 180)
                    {
                        angle -= 180;
                    }
                    else
                    {
                        angle += 180;
                    }
                    steps.RemoveAt(steps.Count - 1);
                    set_rotate_step(angle);
                    break;
                case "move_x":
                    x_move = blade_absolute.X - lower.X;
                    float toolWidth = viewmodels.VM_tools.toolBox.equipped[0].tool.thickness;
                    //make the blade cut from inside outside or ontop
                    //becuase the defualt is to cut from the left of the line for 
                    switch (UC_machine_table.instance.checkedToolPosition)
                    {
                        case "Inside":
                            x_move += toolWidth;
                            toolWidth = -toolWidth;
                            break;
                        case "Ontop":
                            x_move += toolWidth / 2;
                            toolWidth = toolWidth / 2;
                            break;
                            //don't need to change value for outside
                    }
                    //true if the shape is to the right of the tool
                    if (PolygonMethods.GetIsPointIntersect(VM_shape.instance.polygon, new Point(lower.X + 1, lower.Y), new Point(lower.X + 1, blade_absolute.Y)))
                    {
                        x_move += toolWidth;

                    }


                    set_move(x: x_move);
                    break;
                case "move_y":
                    y_move = blade_absolute.Y - lower.Y;
                    set_move(y: y_move);
                    break;
                case "move_y_by_margin":
                    var top_of_margin = points.get_absolute_center(VM_material.instance.elpMargin, Application.Current.MainWindow).Y + VM_material.instance.elpMarginSize / 2;
                    
                    y_move = blade_absolute.Y - top_of_margin;
                    set_move(y: y_move);
                    break;
                case "move_x_by_margin":
                    x_move = 50;

                    set_move(x: x_move);
                    break;
                #endregion

                #region drill
                case "drill_rotate":
                    switch_to_drill();
                    set_rotate_step(0, "Drill");
                    break;
                case "drill_move_x":
                    x_move = drill_absolute.X - lower.X;
                    set_move(x: x_move, toolname: "Drill");
                    break;
                case "drill_move_y":
                    y_move = drill_absolute.Y - lower.Y;
                    counterI++;
                    set_move(y: y_move, toolname: "Drill");
                    break;
                case "drill_move_y_by_margin":
                    top_of_margin = points.get_absolute_center(VM_material.instance.elpMargin, Application.Current.MainWindow).Y + VM_material.instance.elpMarginSize / 2;

                    y_move = blade_absolute.Y - top_of_margin;
                    set_move(y: y_move, toolname: "Drill");
                    break;
                case "drill_move_x_by_margin":
                    x_move = -50;

                    set_move(x: x_move, toolname: "Drill");
                    break; 
                    #endregion

            }
            storyboard.Begin();
        }
        private void animation_step_completed(object sender, EventArgs e)
        {
            count_finished++;
            if (count_finished == req_finished)
            {
                switch (curr_action)
                {
                    #region saw
                    case "rotate":
                        Point p1, p2;
                        p1 = PolygonMethods.GetPointAbsolute(VM_shape.instance.polygon, curr_index, Application.Current.MainWindow);
                        if (curr_index < angles.Length - 1)
                        {
                            p2 = PolygonMethods.GetPointAbsolute(VM_shape.instance.polygon, curr_index + 1, Application.Current.MainWindow);
                        }
                        else
                        {
                            p2 = PolygonMethods.GetPointAbsolute(VM_shape.instance.polygon, 0, Application.Current.MainWindow);
                        }
                        p1.Y = Application.Current.MainWindow.ActualHeight - p1.Y;
                        p2.Y = Application.Current.MainWindow.ActualHeight - p2.Y;
                        var higher = points.get_higher(p1, p2);

                        if (PolygonMethods.GetIsPointIntersect(VM_shape.instance.polygon, higher, new Point(higher.X, blade_absolute.Y)))
                        {
                            curr_action = "rotate_opposite";
                        }
                        else
                        {
                            curr_action = "move_x";
                        }
                        break;
                    case "rotate_opposite":
                        curr_action = "move_x";
                        break;
                    case "move_x":
                        curr_action = "move_y";
                        break;
                    case "move_y":
                        curr_action = "move_y_by_margin";
                        break;
                    case "move_y_by_margin":
                        var top_of_margin = points.get_absolute_center(VM_material.instance.elpMargin, Application.Current.MainWindow).Y
                            + VM_material.instance.elpMarginSize / 2;
                        if (blade_absolute.Y - Math.Round(top_of_margin) == 0)
                            curr_action = "move_x_by_margin";
                        break;
                    #endregion

                    #region switch between drill and saw
                    case "move_x_by_margin":
                        if (curr_index + 1 < angles.Length)
                            curr_action = "rotate";
                        else
                        {
                            //this is when the animation turns to drilling
                            curr_action = "drill_rotate";
                        }
                        curr_index += 1;
                        break; 
                    #endregion

                    #region drill
                    case "drill_rotate":
                        curr_action = "drill_move_x";
                        break;
                    case "drill_move_x":
                        curr_action = "drill_move_y";
                        break;
                    case "drill_move_y":
                        //stops the animation to emulate drilling
                        storyboard.BeginTime = TimeSpan.FromSeconds(drill_time);
                        curr_action = "drill_move_y_by_margin";
                        break;
                    case "drill_move_y_by_margin":
                        //returns to normal
                        storyboard.BeginTime = TimeSpan.FromSeconds(0);
                        top_of_margin = points.get_absolute_center(VM_material.instance.elpMargin, Application.Current.MainWindow).Y
                           + VM_material.instance.elpMarginSize / 2;
                        if (drill_absolute.Y - Math.Round(top_of_margin) == 0)
                            curr_action = "drill_move_x_by_margin";
                        break;
                    case "drill_move_x_by_margin":
                        curr_action = "drill_move_x";
                        curr_index++;
                        break; 
                        #endregion
                }
                if (curr_index < angles.Length+drillPoints.Count)
                {
                    saw_step_manage();
                }
                else
                {
                    sawing_finish();
                }
            }
        }

        private void set_animations_time()
        {
            //storyboard.BeginTime = TimeSpan.FromSeconds(1); // creates 1 second delay between animations
            material_angle.BeginTime = TimeSpan.FromSeconds(-target_time);
            material_margin.BeginTime = TimeSpan.FromSeconds(-target_time);

            target_time += speed;
        }
        private void set_rotate_step(double angle, string toolname = "saw")
        {
            material_angle.KeyFrames.Add(animations.get_keyframe_double(angle, target_time));
            material_margin.KeyFrames.Add(animations.get_keyframe_margin(material.Margin, target_time));

            steps.Add(new Saw_step() { action = "rotate", value = angle, tool = toolname });
            TransformGroup temp = UC_material.instance.RenderTransform as TransformGroup;
            RotateTransform temp1 = temp.Children[2] as RotateTransform;
            steps.Add(new Saw_step() { action = "EP_rotation", value = temp1.Angle - UC_machine_table.instance.armTwoimagerender.Angle, tool = toolname });

        }
        private void set_move(double x = 0, double y = 0, string toolname = "Saw")
        {
            var curr_angle = ((material.RenderTransform as TransformGroup).Children[2] as RotateTransform).Angle;
            material_angle.KeyFrames.Add(animations.get_keyframe_double(curr_angle, target_time));
            material_margin.KeyFrames.Add(animations.get_keyframe_margin(margins.update_margin
                (material.Margin, l: x, b: y), target_time));

            if (x != 0)
            {
                steps.Add(new Saw_step() { action = "move_x", value = x / 3 * 4, tool = toolname });
                
                steps.Add(new Saw_step() { action = "FA_rotation", value = UC_machine_table.instance.armOneimagerender.Angle, tool = toolname });
                steps.Add(new Saw_step() { action = "SA_rotation", value = UC_machine_table.instance.armTwoimagerender.Angle, tool = toolname });

                TransformGroup temp = UC_material.instance.RenderTransform as TransformGroup;
                RotateTransform temp1 = temp.Children[2] as RotateTransform;
                steps.Add(new Saw_step() { action = "EP_rotation", value = temp1.Angle + UC_machine_table.instance.armTwoimagerender.Angle, tool = toolname });
            }
            if (y != 0)
            {
                steps.Add(new Saw_step() { action = "move_y", value = y / 3 * 4, tool = toolname });
                
                steps.Add(new Saw_step() { action = "FA_rotation", value = UC_machine_table.instance.armOneimagerender.Angle, tool = toolname });
                steps.Add(new Saw_step() { action = "SA_rotation", value = UC_machine_table.instance.armTwoimagerender.Angle, tool = toolname });

                TransformGroup temp = UC_material.instance.RenderTransform as TransformGroup;
                RotateTransform temp1 = temp.Children[2] as RotateTransform;
                steps.Add(new Saw_step() { action = "EP_rotation", value = temp1.Angle - UC_machine_table.instance.armTwoimagerender.Angle, tool = toolname });

            }
        }
        #endregion

        #region Post Animation
        private void sawing_finish()
        {
            storyboard.Remove();

            VM_machine_table.instance.pincherX = VM_material.instance.uiMaterial.Margin.Left;
            VM_machine_table.instance.pincherY = VM_material.instance.uiMaterial.Margin.Bottom;

            gcode_create();
        }

        #region GCode
        private void gcode_create()
        {
            gcodes = new string[steps.Count];
            double PFA_rotation=0, PSA_rotation=0, PEP_rotation=0;

            string feed_rate = "300";
            for (int i = 0; i < steps.Count; i++)
            {
                switch (steps[i].action)
                {
                    case "rotate":
                        gcodes[i] = $"{steps[i].tool}: G1 C {Math.Round(steps[i].value, 1)} F {feed_rate}";
                        break;
                    case "move_x":
                        gcodes[i] = $"{steps[i].tool}: G1 X {Math.Round(steps[i].value, 2)} F {feed_rate}";
                        break;
                    case "move_y":
                        gcodes[i] = $"{steps[i].tool}: G1 Y {Math.Round(steps[i].value, 2)} F {feed_rate}";
                        break;
                    case "FA_rotation":
                        gcodes[i] = $"FA_rotation:  {PFA_rotation - Math.Round(steps[i].value, 2)} ";
                        PFA_rotation = Math.Round(steps[i].value, 2);
                        break;
                    case "SA_rotation":
                        gcodes[i] = $"SA_rotation: G1 Y {PSA_rotation - Math.Round(steps[i].value, 2)} ";
                        PSA_rotation = Math.Round(steps[i].value, 2);
                        break;
                    case "EP_rotation":
                        gcodes[i] = $"EP_rotation: G1 Y {PEP_rotation - Math.Round(steps[i].value, 2)} ";
                        PEP_rotation = Math.Round(steps[i].value, 2);
                        break;
                }
            }
            VM_machine_table.instance.gCodesCreated = true;
        }
        #endregion

        #endregion

        #endregion
    }
}
