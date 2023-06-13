<template>
  <MessageComponent
    icon="mdi-information-variant-circle-outline"
    description="<strong>There is no data!</strong>"
    :color="WARNING_AMBER_DARKEN_4"
    v-if="chartDataLables.length == 0"
  />
  <v-layout column v-else-if="isMobile">
    <TitleWithInfoComponent
      :title="TITLE"
      :description="DESCRIPTION"
      h_Text="2"
    />
    <v-select
      @change="onSelectionChanged"
      v-model="selectedActivityType"
      :items="activityTypes"
      label="Activity type"
      class="ma-2"
      item-text="name"
      item-value="id"
      return-object
      attach
      outlined
      filled
    ></v-select>
    <BarChartComponent
      :values="chartDataValues"
      :labels="chartDataLables"
      :yaxis="computePercentage"
      class="move-behind"
    />
  </v-layout>
  <v-layout column v-else>
    <h2>{{ TITLE }}</h2>
    <v-select
      @change="onSelectionChanged"
      v-model="selectedActivityType"
      :items="activityTypes"
      label="Activity type"
      class="ma-2"
      item-text="name"
      item-value="id"
      attach
      outlined
      filled
    ></v-select>
    <v-flex class="d-flex flex-row">
      <div class="custom-box ma-6 my-12">
        <p>{{ DESCRIPTION }}</p>
        <v-img
          :src="require(`@/assets/images/diagrams/compute_percentage.svg`)"
          class="ma-3"
          width="150px"
        ></v-img>
      </div>
      <BarChartComponent
        :values="chartDataValues"
        :labels="chartDataLables"
        :yaxis="computePercentage"
        class="move-behind"
      />
    </v-flex>
  </v-layout>
</template>
    <style scoped>
.custom-box {
  width: 20%;
}
.move-behind {
  z-index: 0;
}
</style>
    <script lang="ts">
import MessageComponent from "@/components/shared-components/MessageComponent.vue";
import TitleWithInfoComponent from "@/components/shared-components/TitleWithInfoComponent.vue";
import BarChartComponent from "@/components/shared-components/charts/BarChartComponent.vue";
import Vue from "vue";
import { WARNING_AMBER_DARKEN_4 } from "@/shared/constants";
import { CourseDashboardViewModule } from "@/modules/view-modules";
import { CourseType } from "@/shared/enums";

export default Vue.extend({
  name: "AttendancePerCourseDiagram",
  components: { TitleWithInfoComponent, BarChartComponent, MessageComponent },
  props: {
    courses: {
      type: Array as () => CourseDashboardViewModule[],
      required: true,
    },
    computePercentage: {
      type: Boolean,
      default: false,
    },
  },
  data: function () {
    return {
      WARNING_AMBER_DARKEN_4,
      DESCRIPTION: this.computePercentage
        ? "The diagram shows the percentage of attendances for each course."
        : "The diagram shows the number of attendances for each course.",
      TITLE: this.computePercentage
        ? "Percentage of attendance per course"
        : "Total attendances per course",
      chartDataLables: [] as string[],
      chartDataValues: {},
      selectedActivityType: CourseType.None,
      activityTypes: [
        { id: CourseType.None, name: "All activities" },
        { id: CourseType.Lecture, name: "Lecture" },
        { id: CourseType.Laboratory, name: "Laboratory" },
        { id: CourseType.Seminary, name: "Seminary" },
      ],
    };
  },
  computed: {
    isMobile: function (): boolean {
      return this.$vuetify.breakpoint.mdAndDown;
    },
  },
  created: function (): void {
    this.onSelectionChanged();
  },
  methods: {
    onSelectionChanged: function (): void {
      const result = this._countReports();
      const name = this.computePercentage
        ? "Attendnace percentage(%)"
        : "Number of attendances";

      if (result != null) {
        this.chartDataLables = result.labels;
        this.chartDataValues = [
          {
            name: name,
            data: result.values,
          },
        ];
      }
    },
    _countReports: function (): {
      labels: string[];
      values: number[];
    } {
      let result: {
        labels: string[];
        values: number[];
      } = {
        labels: [],
        values: [],
      };

      this.courses.forEach((course) => {
        result.labels.push(course.courseName);
        let value = 0;
        if (course.reports.length != 0) {
          if (this.computePercentage) {
            //sum percentage for each report
            value = course.reports.reduce(
              (partialSum, a) =>
                a.percentageAttendances[this.selectedActivityType] + partialSum,
              0
            );
            //divide by no of reports
            value /= course.reports.length;
          } else {
            value = course.reports.reduce(
              (partialSum, a) =>
                a.noAttendancesAchieved[this.selectedActivityType] + partialSum,
              0
            );
          }
        }

        result.values.push(Number.parseFloat(value.toFixed(2)));
      });
      return result;
    },
  },
});
</script>