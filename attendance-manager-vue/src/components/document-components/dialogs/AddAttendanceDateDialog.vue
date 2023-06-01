<template>
  <v-card>
    <v-card-title>
      <span class="text-h5">Add course activity</span>
    </v-card-title>
    <v-card-text>
      <v-layout class="pa-4" wrap>
        <DatePickerComponent @save="getDate" />
        <TimePickerComponent @save="getTime" />
        <v-select
          v-model="selectedCourseType"
          :items="courseType"
          label="Activity type"
          prepend-icon="mdi-school"
          required
        ></v-select>
      </v-layout>
    </v-card-text>
    <v-card-actions>
      <v-spacer></v-spacer>
      <v-btn color="blue darken-1" text @click="$emit('close')"> Close </v-btn>
      <v-btn
        color="blue darken-1"
        text
        @click="onSubmit"
        :disabled="date === '' || time === null || selectedCourseType === ''"
      >
        Save
      </v-btn>
    </v-card-actions>
  </v-card>
</template>
<script lang="ts">
import { CourseType } from "@/shared/enums";
import Vue from "vue";
import DatePickerComponent from "@/components/shared-components/DatePickerComponent.vue";
import TimePickerComponent from "@/components/shared-components/TimePickerComponent.vue";
import storeHelper from "@/store/store-helper";

export default Vue.extend({
  name: "AddAttendanceDateDialog",
  components: {
    DatePickerComponent,
    TimePickerComponent,
  },
  data: function () {
    return {
      date: "",
      time: null,
      courseType: [] as string[],
      selectedCourseType: "",
    };
  },
  computed: {
    documentId: function (): number {
      return storeHelper.documentStore.report.reportId;
    },
  },
  created: function () {
    if (
      storeHelper.documentStore.report.noLaboratories <
      storeHelper.documentStore.report.maxNoLaboratories
    ) {
      this.courseType.push(CourseType[CourseType.Laboratory]);
    }
    if (
      storeHelper.documentStore.report.noLessons <
      storeHelper.documentStore.report.maxNoLessons
    ) {
      this.courseType.push(CourseType[CourseType.Lecture]);
    }
    if (
      storeHelper.documentStore.report.noSeminaries <
      storeHelper.documentStore.report.maxNoSeminaries
    ) {
      this.courseType.push(CourseType[CourseType.Seminary]);
    }
  },
  methods: {
    onSubmit: async function (): Promise<void> {
      let response = await storeHelper.documentStore.addCollection(
        `${this.date} ${this.time}`,
        this.selectedCourseType
      );

      if (response) {
        this.$emit("save");
      }
    },
    getDate: function (date): void {
      this.date = date;
    },
    getTime: function (time): void {
      this.time = time;
    },
  },
});
</script>