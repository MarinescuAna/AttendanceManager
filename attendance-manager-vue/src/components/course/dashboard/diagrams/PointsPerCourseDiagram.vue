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
      attach
      outlined
      filled
    ></v-select>
    <BarChartComponent
      :values="chartDataValues"
      :labels="chartDataLables"
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
          :src="require(`@/assets/images/diagrams/compute_points.svg`)"
          class="ma-3"
          width="150px"
        ></v-img>
      </div>
      <BarChartComponent
        :values="chartDataValues"
        :labels="chartDataLables"
      />
    </v-flex>
  </v-layout>
</template>

<style scoped>
.custom-box {
  width: 20%;
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
  name: "PointsPerCourseDiagram",
  components: { TitleWithInfoComponent, BarChartComponent, MessageComponent },
  props: {
    courses: {
      type: Array as () => CourseDashboardViewModule[],
      required: true,
    },
  },
  data: function () {
    return {
      WARNING_AMBER_DARKEN_4,
      DESCRIPTION:
        "The diagram shows the number of bonus points achieved for each course.",
      TITLE: "Total bonus points per course",
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

      if (result != null) {
        this.chartDataLables = result.labels;
        this.chartDataValues = [
          {
            name:'Bonus points',
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
        result.values.push(
            course.reports.reduce(
            (partialSum, a) => a.noPointsAchieved[this.selectedActivityType] + partialSum,
            0
          )
        );
      });
      return result;
    },
  },
});
</script>