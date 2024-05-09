<template>
  <div>
    <div v-if="isLoading">
      <v-layout justify-center>
        <v-progress-circular
          :size="100"
          :width="8"
          color="black"
          indeterminate
        ></v-progress-circular>
      </v-layout>
    </div>
    <div v-else-if="courses.length > 0">
      <!-- The tabs menu -->
      <v-tabs
        v-model="currentTab"
        background-color="transparent"
        color="black"
        centered
      >
        <v-tab> Courses dashboard </v-tab>
        <v-tab> Single course dashboard</v-tab>
        <v-tab> Badges dashboard</v-tab>
      </v-tabs>

      <v-tabs-items v-model="currentTab" class="pa-3 remove-background-color">
        <v-tab-item>
          <v-layout align-center column>
            <ReportsPerCourseDiagram :courses="courses" />
            <AttendancePerCourseDiagram
              :courses="courses"
              :computePercentage="false"
            />
            <AttendancePerCourseDiagram
              :courses="courses"
              :computePercentage="true"
            />
            <PointsPerCourseDiagram :courses="courses" />
          </v-layout>
        </v-tab-item>
        <v-tab-item>
          <v-layout align-center column>
            <v-select
              @change="onSelectionChanged"
              :items="coursesName"
              label="Courses"
              class="ma-2"
              color="black"
              item-text="name"
              item-value="id"
              outlined
              filled
            ></v-select>
            <CourseOverviewTab
              :course="selectedCourse"
            />
          </v-layout>
        </v-tab-item>
        <v-tab-item>
          <v-layout align-center column>
            <v-select
              @change="onSelectionChanged"
              :items="coursesName"
              label="Courses"
              class="ma-2"
              color="black"
              item-text="name"
              item-value="id"
              full-width
              outlined
              filled
            ></v-select>
            <BadgesOverviewTab
              :reports="selectedReports"
              v-if="selectedReports.length > 0"
            />
            <MessageComponent
              description="There are no badges because you don't select a course or because there is no report defined for this course."
              fontWeight="bold"
              fontSize="20px"
              icon="mdi-information-variant-circle-outline"
              :color="WARNING_AMBER_DARKEN_4"
              v-else
            />
          </v-layout>
        </v-tab-item>
      </v-tabs-items>
    </div>
    <div v-else>
      <MessageComponent
        description="There are no courses that you create. If you want to create a course, go to <a href='\\course'>Create new course</a> page."
        fontWeight="bold"
        fontSize="20px"
      />
    </div>
  </div>
</template>
  
  <style lang="css" scoped>
.remove-background-color {
  background-color: transparent;
}
</style>
<script lang="ts">
import MessageComponent from "@/components/shared-components/MessageComponent.vue";
import { Toastification } from "@/plugins/vue-toastification";
import Vue from "vue";
import CourseService from "@/services/course.service";
import {
  CourseDashboardViewModule,
  ReportDashboardDto,
} from "@/modules/view-modules";
import ReportsPerCourseDiagram from "@/components/course/dashboard/diagrams/ReportsPerCourseDiagram.vue";
import AttendancePerCourseDiagram from "@/components/course/dashboard/diagrams/AttendancePerCourseDiagram.vue";
import PointsPerCourseDiagram from "@/components/course/dashboard/diagrams/PointsPerCourseDiagram.vue";
import BadgesOverviewTab from "@/components/course/dashboard/BadgesOverviewTab.vue";
("@/components/course/dashboard/ ");
import { WARNING_AMBER_DARKEN_4 } from "@/shared/constants";
import CourseOverviewTab from "@/components/course/dashboard/CourseOverviewTab.vue";

interface CourseDto {
  id: number;
  name: string;
}

export default Vue.extend({
  name: "DashboardView",
  components: {
    MessageComponent,
    ReportsPerCourseDiagram,
    AttendancePerCourseDiagram,
    PointsPerCourseDiagram,
    BadgesOverviewTab,
    CourseOverviewTab,
  },
  data: function () {
    return {
      WARNING_AMBER_DARKEN_4,
      currentTab: 0,
      /** Use this boolean to display the progress circular component */
      courses: [] as CourseDashboardViewModule[],
      isLoading: true,
      // Add a flag to track if data fetching is successful
      isFetchSuccessful: false,
      selectedReports: [] as ReportDashboardDto[],
      selectedCourse: {} as CourseDashboardViewModule,
    };
  },
  watch: {
    currentTab: function (): void {
      this.selectedReports = [];
      this.selectedCourse = {} as CourseDashboardViewModule;
    },
  },
  computed: {
    reports: function (): ReportDashboardDto[] {
      return this.courses.flatMap((s) => s.reports);
    },
    coursesName: function (): CourseDto[] {
      let result: CourseDto[] =
        this.currentTab == 2 ? [{ id: 0, name: "Overview" }] : [];
      this.courses.forEach((c) => {
        result.push({ id: c.courseId, name: c.courseName });
      });
      return result;
    },
  },
  /** Load all the documents from the API */
  created: async function () {
    // Fetch data with a timeout of 1 minute
    const fetchDataWithTimeout = async () => {
      try {
        this.courses = await CourseService.getDashboardAsync();
        this.isFetchSuccessful = true;
      } catch (error) {
        Toastification.simpleError("An error occurred during data fetching.");
      } finally {
        this.isLoading = false;
      }
    };

    // Start fetching data
    fetchDataWithTimeout();

    // Set a timeout to hide the loader if data is not fetched
    setTimeout(() => {
      if (!this.isFetchSuccessful) {
        this.isLoading = false;
        Toastification.simpleError("Data fetching timeout");
      }
    }, 100000);
  },
  methods: {
    onSelectionChanged: function (value): void {      
      if (this.currentTab == 2) {
        if (value == 0) {
          this.selectedReports = this.courses.flatMap((r) => r.reports);
        } else {
          this.selectedReports = this.courses.find(
            (c) => c.courseId == value
          )!.reports;
        }
      }else if(this.currentTab == 1){
        this.selectedCourse = this.courses.find(c=>c.courseId == value)!;
      }
    },
  },
});
</script>