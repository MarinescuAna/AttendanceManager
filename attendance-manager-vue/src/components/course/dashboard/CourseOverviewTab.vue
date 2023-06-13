<template>
  <v-layout v-if="course.reports.length != 0" align-center column>
    <h1 class="font-weight-bold ma-6">
      Reports for {{ course.courseName }} course
    </h1>
    <InvolvementsPerReportDiagram
      :reports="course.reports"
      :computePercentage="true"
      :courseTitle="course.courseName"
    />
    <InvolvementsPerReportDiagram
      :reports="course.reports"
      :computeNoAttendances="true"
      :courseTitle="course.courseName"
    />
    <InvolvementsPerReportDiagram
      :reports="course.reports"
      :computePoints="true"
      :courseTitle="course.courseName"
    />
    <InvolvementsPerYearDiagram
      :reports="course.reports"
      :computeNoAttendances="true"
      :courseTitle="course.courseName"
    />
    <InvolvementsPerYearDiagram
      :reports="course.reports"
      :computePoints="true"
      :courseTitle="course.courseName"
    />
  </v-layout>
  <MessageComponent
    description="There is no report defined for this course."
    fontWeight="bold"
    fontSize="20px"
    icon="mdi-information-variant-circle-outline"
    :color="WARNING_AMBER_DARKEN_4"
    v-else
  />
</template>

<script lang="ts">
import { CourseDashboardViewModule } from "@/modules/view-modules";
import Vue from "vue";
import InvolvementsPerReportDiagram from "./diagrams/InvolvementsPerReportDiagram.vue";
import { WARNING_AMBER_DARKEN_4 } from "@/shared/constants";
import InvolvementsPerYearDiagram from "./diagrams/InvolvementsPerYearDiagram.vue";

export default Vue.extend({
  name: "CourseOverviewTab",
  components: { InvolvementsPerReportDiagram, InvolvementsPerYearDiagram },
  props: {
    course: {
      type: Object as () => CourseDashboardViewModule,
      required: true,
    },
  },
  data: function () {
    return {
      WARNING_AMBER_DARKEN_4,
    };
  },
});
</script>