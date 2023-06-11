<template>
  <v-layout column>
    <TitleWithInfoComponent
      title="Students interest regarding the selected activity type"
      description="Percentage of each student's interest related to a course(this is related to the students that are members of the document): calculate a weighted average of the attendances and bonus points, where the attendances and bonus points are weighted by their relative importance(this means that we will add new fields at the created document, where the teacher should be forced to add the percentage of the importance of attendances and bonus points):
<strong>Weighted_average=(% * attendance)+(% * bonus_points)</strong>. This average will be then divided by the total possible score of the course <strong>Percentage = (weighted_average/total_possible_score) * 100</strong>.
"
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
      v-if="chartDataLabels.length != 0"
      :values="chartDataValues"
      :labels="chartDataLabels"
      class="move-behind"
    />
    <MessageComponent
      icon="mdi-information-variant-circle-outline"
      description="<strong>There is no data for this type of activity!</strong>"
      :color="WARNING_AMBER_DARKEN_4"
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
import { CourseType } from "@/shared/enums";
import storeHelper from "@/store/store-helper";
import Vue from "vue";
import {WARNING_AMBER_DARKEN_4} from "@/shared/constants";
import { InvolvementViewModule } from "@/modules/view-modules";

interface StudentInterestModule {
  email: string;
  value: number;
}

export default Vue.extend({
  name: "StudentsInterestDiagramComponent",
  components: { BarChartComponent, TitleWithInfoComponent, MessageComponent },
  props: {
    involvements: {
      type: Array as () => InvolvementViewModule[],
      required: true,
    },
  },
  data: function () {
    return {
      WARNING_AMBER_DARKEN_4,
      selectedActivityType: CourseType.None,
      activityTypes: [
        { id: CourseType.None, name: "All activities" },
        { id: CourseType.Lecture, name: "Lecture" },
        { id: CourseType.Laboratory, name: "Laboratory" },
        { id: CourseType.Seminary, name: "Seminary" },
      ],
      chartDataLabels: [] as string[],
      chartDataValues: {},
    };
  },
  created: function (): void {
    this._getResults(CourseType.None);
  },
  methods: {
    onSelectionChanged: function (): void {
      this._getResults(this.selectedActivityType["id"]);
    },
    _getResults: function (type: CourseType): void {
      let finalResult: StudentInterestModule[] = [];
      if (type == CourseType.None) {
        // initialize the list with the values computed for lectures
        finalResult = this._computeStudentsInterest(CourseType.Lecture);

        //compute values for laboratories
        this._computeStudentsInterest(CourseType.Laboratory).forEach(
          (element) => {
            let targetObject = finalResult.find(
              (e) => e.email === element.email
            );
            if (targetObject != undefined) {
              targetObject.value += element.value;
            } else {
              finalResult.push(element);
            }
          }
        );

        //compute values for seminaries
        this._computeStudentsInterest(CourseType.Seminary).forEach(
          (element) => {
            let targetObject = finalResult.find(
              (e) => e.email === element.email
            );
            if (targetObject != undefined) {
              targetObject.value += element.value;
            } else {
              finalResult.push(element);
            }
          }
        );

        //divide all the values by 3
        finalResult.forEach((element) => {
          element.value /= 3;
        });
      } else {
        finalResult = this._computeStudentsInterest(type);
      }

      this.chartDataLabels = finalResult.map((e) => e.email);
      this.chartDataValues = [
        {
          data: finalResult.map((e) => Number.parseFloat(e.value.toFixed(2))),
        },
      ];
    },
    /**
     * Data related to students interest
     * NOTE:
     * Students interest is computed like this:
     *   - calculate a weighted average of the attendances and bonus points,
     *   where the attendances and bonus points are weighted by their relative importance (the fields requiered when the document is created)
     *   - Weighted_average=(% * attendance)+(% * bonus_points)
     *   - This average will be then divided by the total possible score of the course
     *   - Percentage = (weighted_average/total_possible_score) * 100
     */
    _computeStudentsInterest: function (
      type: CourseType
    ): StudentInterestModule[] {
      let objFinalResult: StudentInterestModule[] = [];

      //get all the involvements according to the activity type
      const involvements: InvolvementViewModule[] =
        type == CourseType.None
          ? this.involvements
          : this.involvements.filter((a) => a.activityType == type);

      if (involvements.length == 0) {
        return [];
      }
      //get the weight form the store
      const attendanceWeight =
        storeHelper.documentStore.report.attendanceImportance;
      const pointsWeight =
        storeHelper.documentStore.report.bonusPointsImportance;

      //get a list with all the involvements gouped by emails
      const involvementsGrouped = involvements.reduce((groups, item) => {
        const group = groups[item.student] || [];
        group.push(item);
        groups[item.student] = group;
        return groups;
      }, {});

      let maxBonusPoints = 0;
      //for each student, compute the number of attendances and the sum of bonus points for the activity
      //and compute the weighted_average=(% * attendance)+(% * bonus_points)
      for (var email in involvementsGrouped) {
        const computedBonusPointsSum = involvementsGrouped[email].reduce(
          (partialSum, a) => a.bonusPoints + partialSum,
          0
        );
        objFinalResult.push({
          value:
            involvementsGrouped[email].length * attendanceWeight +
            pointsWeight * computedBonusPointsSum,
          email: email,
        });
        if (computedBonusPointsSum > maxBonusPoints) {
          maxBonusPoints = computedBonusPointsSum;
        }
      }

      //compute the maximum number of points that can be obtain by students
      let possibleScore = 0;
      if (type == CourseType.Laboratory) {
        possibleScore =
          storeHelper.documentStore.report.maxNoLaboratories *
            attendanceWeight +
          maxBonusPoints * pointsWeight;
      } else if (type == CourseType.Seminary) {
        possibleScore =
          storeHelper.documentStore.report.maxNoSeminaries *
            attendanceWeight +
          maxBonusPoints * pointsWeight;
      }
      if (type == CourseType.Lecture) {
        possibleScore =
          storeHelper.documentStore.report.maxNoLessons *
            attendanceWeight +
          maxBonusPoints * pointsWeight;
      }

      //compute the final score: Percentage = (weighted_average/total_possible_score) * 100
      objFinalResult.forEach((score) => {
        score.value =
          possibleScore == 0 ? 0 : (score.value / possibleScore) * 100;
      });

      return objFinalResult;
    },
  },
});
</script>