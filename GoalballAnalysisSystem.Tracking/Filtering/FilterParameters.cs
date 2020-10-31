using System;
using System.Collections.Generic;
using System.Text;

namespace GoalballAnalysisSystem.Tracking.Filtering
{
    static class FilterParameters
    {
        const int H_MIN = 94;
        const int H_MAX = 118;

        const int S_MIN = 100;
        const int S_MAX = 250;

        const int V_MIN = 98;
        const int V_MAX = 255;

        const int ADDEDHEIGHT = 0;

        const int MIN_BALL_SIZE = 700;
        const int MAX_BALL_SIZE = 1800;

        const int FRAME_WIDTH = 640;
        const int FRAME_HEIGHT = 480;

        const int MAX_NUM_OBJECTS = 10;

        const int MIN_OBJECT_AREA = 20 * 20;
        const int MAX_OBJECT_AREA = (int)((double)FRAME_HEIGHT * FRAME_WIDTH / 1.5);
    }
}
