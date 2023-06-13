<template>
  <MessageComponent
    icon="mdi-information-variant-circle-outline"
    :description="errorText"
    :color="WARNING_AMBER_DARKEN_4"
    v-if="chartDataLables.length == 0"
  />
  <v-layout column v-else-if="isMobile">
    <TitleWithInfoComponent
      :title="titleText"
      :description="descriptionText"
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
      class="move-behind"
    />
  </v-layout>
  <v-layout column v-else>
    <h2>{{ titleText }}</h2>
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
        <p>{{ descriptionText }}</p>
        <v-img
          :src="require(`@/assets/images/diagrams/year_diagram.svg`)"
          class="ma-3"
          width="150px"
        ></v-img>
      </div>
      <BarChartComponent
        :values="chartDataValues"
        :labels="chartDataLables"
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
import { ReportDashboardDto } from "@/modules/view-modules";
import { CourseType } from "@/shared/enums";

export default Vue.extend({
  name: "InvolvementsPerYearDiagram",
  components: { TitleWithInfoComponent, BarChartComponent, MessageComponent },
  props: {
    reports: {
      type: Array as () => ReportDashboardDto[],
      required: true,
    },
    courseTitle: {
      type: String,
      required: true,
    },
    computeNoAttendances: {
      type: Boolean,
      default: false,
    },
    computePoints: {
      type: Boolean,
      default: false,
    },
  },
  data: function () {
    return {
      WARNING_AMBER_DARKEN_4,
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
    descriptionText: function (): string {
      if (this.computeNoAttendances) {
        return `The current diagram displays the number of attendances per year for ${this.courseTitle}`;
      }
      if (this.computePoints) {
        return `The current diagram displays the number of points acumulated per year for ${this.courseTitle}`;
      }

      return `Something went wrong and we have no description`;
    },
    titleText: function (): string {
      if (this.computeNoAttendances) {
        return `Number of attendances per year for ${this.courseTitle}`;
      }
      if (this.computePoints) {
        return `Number of points acumulated per year for ${this.courseTitle}`;
      }

      return `Something went wrong and we have no title`;
    },
    errorText: function (): string {
      if (this.computeNoAttendances) {
        return `There is no report and we cannot compute the diagram for number of attendances per year for ${this.courseTitle}`;
      }
      if (this.computePoints) {
        return `There is no report and we cannot compute the diagram for number of points acumulated per year for ${this.courseTitle}`;
      }

      return `Something went wrong and we have no error message`;
    },
  },
  created: function (): void {
    this.onSelectionChanged();
  },
  watch: {
    reports: function (): void {
      this.onSelectionChanged();
    },
  },
  methods: {
    onSelectionChanged: function (): void {
      let result: {
        labels: string[];
        values: number[];
      } = {
        labels: [],
        values: [],
      };
      let name = "";

      if (this.computeNoAttendances) {
        result = this._dataDiagram();
        name = "Number of attendances";
      }
      if (this.computePoints) {
        result = this._dataDiagram();
        name = "Bonus points";
      }

      this.chartDataLables = result.labels;
      this.chartDataValues = [
        {
          name: name,
          data: result.values,
        },
      ];
    },
    _dataDiagram: function (): {
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

      //groupe reports per year
      const reportsGrouped = this.reports.reduce((groups, item) => {
        const group = groups[item.creationYear] || [];

        let value = 0;
        if (this.computeNoAttendances) {
          value = item.noAttendancesAchieved[this.selectedActivityType];
        }
        if (this.computePoints) {
          value = item.noPointsAchieved[this.selectedActivityType];
        }

        if (group.length == 0) {
          group.push(value);
        } else {
          group[0] += item.noAttendancesAchieved[this.selectedActivityType];
        }
        groups[item.creationYear] = group;
        return groups;
      }, {});

      for (var year in reportsGrouped) {
        result.labels.push(year);
        result.values.push(reportsGrouped[year][0]);
      }

      return result;
    },
  },
});
</script>