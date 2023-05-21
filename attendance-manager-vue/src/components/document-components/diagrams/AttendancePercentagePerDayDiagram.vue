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
    <LineChartComponent
      v-if="diagramData != null"
      :chartData="diagramData.value"
      description="The values represents the persentage of users attendance per day computed as follow: <br/> <strong>percent = number_of_attedances_per_day/total_students*100</strong>"
      :xAxiesLabels="diagramData.labels"
      title="Attendance percentage per day for selected activity"
      class="move-behind"
    />
    <MessageComponent icon="mdi-information-variant-circle-outline" description="<strong>There is no data for this type of activity!</strong>" v-else />
  </v-layout>
</template>
<style scoped>
.move-behind {
  z-index: 0;
}
</style>
<script lang="ts">
import MessageComponent from "@/components/shared-components/MessageComponent.vue";
import LineChartComponent from "@/components/shared-components/charts/LineChartComponent.vue";
import { CourseType } from "@/shared/enums";
import storeHelper from "@/store/store-helper";
import Vue from "vue";

export default Vue.extend({
  name: "AttendancePercentagePerDayDiagram",
  components: { LineChartComponent, MessageComponent },
  data: function () {
    return {
      selectedActivityType: CourseType.Lecture,
      activityTypes: [
        { id: CourseType.Lecture, name: "Lecture" },
        { id: CourseType.Laboratory, name: "Laboratory" },
        { id: CourseType.Seminary, name: "Seminary" },
      ],
      diagramData: {
        value: {},
        labels: [] as string[],
      },
    };
  },
  computed: {
    /**
     * Compute the percentage of students that attend a course
     * NOTE:
     * Take the number of attendances for each day, divide by the number of students
     * that are supposed to attend that course on that day, and then multiply by 100 to get the percentage
     */
    attendancePercentagePerDay: function (): object {
      return [
        {
          name: "Attendance percentage per day for all activity",
          type: "column",
          data: storeHelper.documentStore.documentDetails.documentDashboard.attendancePercentage.map(
            (d) => d.percentage
          ),
        },
      ];
    },
    attendancePercentageLabels: function (): string[] {
      return storeHelper.documentStore.documentDetails.documentDashboard.attendancePercentage.map(
        (d) => d.datetime
      );
    },
  },
  created: function (): void {
    this.diagramData = this._computePercentagePerActivityType(
      CourseType.Lecture
    );
  },
  methods: {
    onSelectionChanged: function (): void {
      this.diagramData = this._computePercentagePerActivityType(
        this.selectedActivityType["id"]
      );
    },
    _computePercentagePerActivityType: function (type: CourseType): {
      value: object;
      labels: string[];
    } {
      //filter involvments by given type
      const involvements = storeHelper.involvementStore.involvements.filter(
        (s) => s.activityType == type
      );

      if (involvements.length == 0) {
        return null!;
      }

      //get the number of students that should be present
      const numberOfAttendances =
        storeHelper.documentStore.documentDetails.totalAttendances.length;

      //group involvments bt collection id
      const involvementsGrouped = involvements.reduce((groups, item) => {
        const group = groups[item.collectionId] || [];
        group.push(item);
        groups[item.collectionId] = group;
        return groups;
      }, {});

      //get all the collections
      const collections =
        storeHelper.documentStore.documentDetails.attendanceCollections;
      const percentages: { label: string; value: number }[] = [];
      for (var collectionId in involvementsGrouped) {
        percentages.push({
          value:
            (involvementsGrouped[collectionId].length / numberOfAttendances) *
            100,
          label: collections.find(
            (d) => d.attendanceCollectionId.toString() == collectionId
          )!.activityTime,
        });
      }

      return {
        value: [
          {
            name: "Percentage per selected day(%)",
            type: "line",
            data: percentages.map((m) => m.value.toFixed(2)),
          },
        ],
        labels: percentages.map((m) => m.label),
      };
    },
  },
});
</script>