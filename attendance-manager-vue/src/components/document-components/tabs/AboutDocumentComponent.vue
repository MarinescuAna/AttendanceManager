<template>
  <v-container>
    <strong>Created at </strong>{{ document.creationDate }} (Last update at
    {{ document.updateOn }})<br />
    <strong>Course name:</strong> {{ document.courseName }}<br />
    <strong>Created by:</strong> {{ document.createdBy }}<br />
    <strong>Specialization name:</strong> {{ document.specializationName }}
    <br />
    <strong>Students' enrollment year:</strong>{{ document.enrollmentYear }}
    <br />
    <strong>Maximum number of lectures that will be held:</strong>
    {{ document.maxNoLessons }}({{ document.noLessons }} held by now)
    <br />
    <strong>Maximum number of laboratories that will be held:</strong>
    {{ document.maxNoLaboratories }}({{ document.noLaboratories }} held by now)
    <br />
    <strong>Maximum number of seminaries that will be held:</strong>
    {{ document.maxNoSeminaries }}({{ document.noSeminaries }} held by now)
    <br />
    <strong>Attendance weight:</strong> {{ document.attendanceImportance }}%
    <br />
    <strong>Bonus points weight:</strong>
    {{ document.bonusPointsImportance }}%
  </v-container>
</template>

<script lang="ts">
import { ReportViewModule } from "@/modules/document";
import storeHelper from "@/store/store-helper";
import Vue from "vue";

export default Vue.extend({
  name: "AboutDocumentComponent",
  computed: {
    /** Get document details from the store */
    document: function (): ReportViewModule {
      return storeHelper.documentStore.report;
    },
  },
});
</script>