<template>
  <v-container class="ma-5">
    <h1 class="ma-5">{{ document.title }}</h1>
    <v-divider class="ma-5"></v-divider>
    <h2>Current report information</h2>
    <strong>Course name:</strong> {{ document.courseName }}<br />
    <strong>Created at </strong>{{ document.creationDate }} (Last update at
    <span :title="document.updatedOn">{{
      getRelativeTime(document.updatedOn)
    }}</span
    >)<br />

    <strong>Created by:</strong> {{ document.createdBy }}<br />
    <strong>Specialization name:</strong> {{ document.specializationName }}
    <br />

    <strong>Students' enrollment year:</strong>{{ document.enrollmentYear }}
    <br />
    <br />
    <v-layout justify-center wrap>
      <v-flex  class="ma-5">
        <strong class="ma-5">Attendance weight </strong>
        <v-progress-circular
          :rotate="360"
          :size="100"
          :width="15"
          :value="document.attendanceImportance"
          :color="WARNING_AMBER_DARKEN_4"
        >
          {{ document.attendanceImportance }}%
        </v-progress-circular></v-flex
      >
      <v-flex  class="ma-5">
        <strong class="ma-5">Bonus points weight </strong>
        <v-progress-circular
          :rotate="360"
          :size="100"
          :width="15"
          :value="document.bonusPointsImportance"
          :color="WARNING_AMBER_DARKEN_4"
        >
          {{ document.bonusPointsImportance }}%
        </v-progress-circular>
      </v-flex>
    </v-layout>
    <v-divider class="ma-5"></v-divider>
    <h2>Class activity status</h2>
    <v-layout wrap>
      <v-flex v-if="document.maxNoLessons > 0">
        <strong class="ma-5">Lectures</strong>
        <v-tooltip bottom>
          <template v-slot:activator="{ on, attrs }">
            <v-progress-circular
              :size="100"
              :width="15"
              :value="document.noLessons/document.maxNoLessons * 100"
              class="ma-5"
              :color="WARNING_AMBER_DARKEN_4"
              v-bind="attrs"
              v-on="on"
            >
              {{ document.noLessons }}/{{ document.maxNoLessons }}
            </v-progress-circular>
          </template>
          <span
            >The number of lectures that were held and the total of them</span
          >
        </v-tooltip>
      </v-flex>
      <v-flex v-if="document.maxNoLaboratories > 0">
        <strong class="ma-5">Laboratories</strong>
        <v-tooltip bottom>
          <template v-slot:activator="{ on, attrs }">
            <v-progress-circular
            :size="100"
              :width="15"
              :value="document.noLaboratories/document.maxNoLaboratories * 100"
              class="ma-5"
              :color="WARNING_AMBER_DARKEN_4"
              v-bind="attrs"
              v-on="on"
              >{{ document.noLaboratories }}/{{ document.maxNoLaboratories }}
            </v-progress-circular>
          </template>
          <span
            >The number of laboratories that were held and the total of
            them</span
          >
        </v-tooltip>
      </v-flex>
      <v-flex v-if="document.maxNoSeminaries > 0">
        <strong class="ma-5">Seminary</strong>
        <v-tooltip bottom>
          <template v-slot:activator="{ on, attrs }">
            <v-progress-circular
            :size="100"
              :width="15"
              :value="document.noSeminaries/document.maxNoSeminaries*100"
              class="ma-5"
              :color="WARNING_AMBER_DARKEN_4"
              v-bind="attrs"
              v-on="on"
              >{{ document.noSeminaries }}/{{ document.maxNoSeminaries }}
            </v-progress-circular>
          </template>
          <span
            >The number of seminaries that were held and the total of them</span
          >
        </v-tooltip>
      </v-flex>
    </v-layout>
  </v-container>
</template>

<script lang="ts">
import storeHelper from "@/store/store-helper";
import Vue from "vue";
import moment from "moment";
import { WARNING_AMBER_DARKEN_4 } from "@/shared/constants";
import { ReportViewModule } from "@/modules/view-modules";

export default Vue.extend({
  name: "AboutDocumentComponent",
  data: function () {
    return {
      WARNING_AMBER_DARKEN_4,
    };
  },
  computed: {
    /** Get document details from the store */
    document: function (): ReportViewModule {
      return storeHelper.documentStore.report;
    },
  },
  methods: {
    getRelativeTime(updateOn: string) {
      return moment(new Date(updateOn)).fromNow();
    },
  },
});
</script>