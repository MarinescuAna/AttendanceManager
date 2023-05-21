<template>
  <v-layout column>
    <h2>Attendance percentage per day</h2>
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
    <PieChartComponent
      v-if="chartData != null"
      description="The diagram consists into all the attendances acumulated by each students, grouped by the total attendance."
      :dataSeries="chartData.values"
      :labels="chartData.labels"
      title="Total attendances acumulated by each student for selected activity"
      class="move-behind"
    />
    <MessageComponent
      icon="mdi-information-variant-circle-outline"
      description="<strong>There is no data for this type of activity!</strong>"
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
import PieChartComponent from "@/components/shared-components/charts/PieChartComponent.vue";
import { CourseType } from "@/shared/enums";
import storeHelper from "@/store/store-helper";
import Vue from "vue";
export default Vue.extend({
  name: "TotalAttendancesDiagram",
  components: { PieChartComponent },
  data: function () {
    return {
      selectedActivityType: CourseType.None,
      activityTypes: [
        { id: CourseType.None, name: "All activities" },
        { id: CourseType.Lecture, name: "Lecture" },
        { id: CourseType.Laboratory, name: "Laboratory" },
        { id: CourseType.Seminary, name: "Seminary" },
      ],
      chartData: {} as {
        labels: string[];
        values: number[];
      },
    };
  },
  created: function (): void {
    this.chartData = this._computeTotalAttendances(CourseType.None);
  },
  methods: {
    onSelectionChanged: function (): void {
        this.chartData = null!;
      this.chartData = this._computeTotalAttendances(
        this.selectedActivityType["id"]
      );
      console.log(this.chartData);
      
    },
    /**Count all the attendances per each student and group them by values */
    _computeTotalAttendances: function (type: CourseType): {
      labels: string[];
      values: number[];
    } {
        console.log(type == CourseType.None);
        
      const involvements =
        type == CourseType.None
          ? storeHelper.involvementStore.involvements
          : storeHelper.involvementStore.involvements.filter(
              (d) => d.activityType == type
            );

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

      //group involvments by user email
      const involvementsGrouped = involvements.reduce((groups, item) => {
        const totalAttendances = involvements.filter(
          (d) => d.studentEmail == item.studentEmail
        ).length;
        if ((groups[totalAttendances] || []).length == 0) {
          groups[totalAttendances] = item.studentEmail;
        } else {
          if (!groups[totalAttendances].includes(item.studentEmail)) {
            groups[totalAttendances] += `\n ${item.studentEmail}`;
          }
        }
        return groups;
      }, {});

      for (let email in involvementsGrouped) {
        result.labels.push(involvementsGrouped[email]);
        result.values.push(Number.parseFloat(email));
      }

      return result;
    },
  },
});
</script>