<template>
  <v-layout column>
    <TitleWithInfoComponent
      title="Total attendances acumulated by each student for selected activity"
      description="The diagram consists into all the attendances acumulated by each students, grouped by the total attendance."
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
      v-if="!noData"
      :values="chartDataValues"
      :labels="chartDataLables"
      class="move-behind"
    />
    <MessageComponent
      icon="mdi-information-variant-circle-outline"
      description="<strong>There is no data for this type of activity!</strong>"
      color="transparent"
      v-else
    />
  </v-layout>
</template>

<style scoped>
.move-behind {
  z-index: 0;
}
</style>

<script lang="ts">
import MessageComponent from "@/components/shared-components/MessageComponent.vue";
import TitleWithInfoComponent from "@/components/shared-components/TitleWithInfoComponent.vue";
import BarChartComponent from "@/components/shared-components/charts/BarChartComponent.vue";
import { InvolvementViewModule } from "@/modules/document/involvement";
import { CourseType } from "@/shared/enums";
import Vue from "vue";

export default Vue.extend({
  name: "TotalAttendancesDiagram",
  components: { TitleWithInfoComponent, BarChartComponent, MessageComponent },
  props: {
    involvements: {
      type: Array as () => InvolvementViewModule[],
      required: true,
    },
  },
  data: function () {
    return {
      selectedActivityType: CourseType.None,
      activityTypes: [
        { id: CourseType.None, name: "All activities" },
        { id: CourseType.Lecture, name: "Lecture" },
        { id: CourseType.Laboratory, name: "Laboratory" },
        { id: CourseType.Seminary, name: "Seminary" },
      ],
      chartDataLables: [] as string[],
      chartDataValues: {},
      noData: false,
    };
  },
  created: function (): void {
    const result = this._computeTotalAttendances(CourseType.None);

    if (result != null) {
      this.chartDataLables = result.labels;
      this.chartDataValues = [
        {
          data: result.values,
        },
      ];
      this.noData = false;
    }
    {
      this.noData = true;
    }
  },
  methods: {
    onSelectionChanged: function (): void {
      this.chartDataLables = null!;
      this.chartDataValues = null!;
      const result = this._computeTotalAttendances(
        this.selectedActivityType["id"]
      );
      this.chartDataLables = result.labels;
      this.chartDataValues = [
        {
          data: result.values,
        },
      ];
    },
    /**Count all the attendances per each student and group them by values */
    _computeTotalAttendances: function (type: CourseType): {
      labels: string[];
      values: number[];
    } {
      const involvements =
        type == CourseType.None
          ? this.involvements
          : this.involvements.filter((d) => d.activityType == type);

      if (involvements.length == 0) {
        return null!;
      }

      let result: {
        labels: string[];
        values: number[];
      } = {
        labels: [],
        values: [],
      };

      //group involvements by user email
      const involvementsGrouped = involvements.reduce((groups, item) => {
        if ((groups[item.student] || []).length == 0) {
          groups[item.student] = involvements.filter(
            (d) => d.student == item.student
          ).length;
        }
        return groups;
      }, {});

      for (let email in involvementsGrouped) {
        result.labels.push(email);
        result.values.push(Number.parseFloat(involvementsGrouped[email]));
      }

      return result;
    },
  },
});
</script>