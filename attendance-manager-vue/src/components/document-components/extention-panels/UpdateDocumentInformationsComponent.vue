<template>
  <v-expansion-panel>
    <v-expansion-panel-header class="font-weight-bold"
      >Update the information of the current report</v-expansion-panel-header
    >
    <v-expansion-panel-content>
      <validation-observer ref="observer" v-slot="{ invalid, handleSubmit }">
        <v-form @submit.prevent="handleSubmit(onSubmitAsync)">
          <v-layout column>
            <validation-provider
              name="document title"
              v-slot="{ errors }"
              :rules="rules.required"
            >
              <v-textarea
                v-model="documentTitle"
                type="text"
                counter
                rows="2"
                maxlength="128"
                label="Involvement report title"
                prepend-icon="mdi-pencil"
                :error-messages="errors"
                required
                color="black"
              />
            </validation-provider>
            <v-select
              :items="courses"
              label="Course"
              v-model="selectedCourse"
              required
              prepend-icon="mdi-school"
              item-text="name"
              item-value="courseId"
              color="black"
              return-object
            ></v-select>
            <v-layout row class="pa-3">
              <v-flex xs12 md4>
                <validation-provider
                  name="maximum number of lectures"
                  v-slot="{ errors }"
                  :rules="rules.between_0_30"
                >
                  <v-text-field
                    v-model="maxNoLessons"
                    type="number"
                    label="Maximum number of lectures that will be held "
                    prepend-icon="mdi-numeric"
                    :error-messages="errors"
                    color="black"
                    required
                  />
                </validation-provider>
              </v-flex>
              <v-flex xs12 md4>
                <validation-provider
                  name="maximum number of laboratories"
                  v-slot="{ errors }"
                  :rules="rules.between_0_30"
                >
                  <v-text-field
                    v-model="maxNoLaboratories"
                    type="number"
                    label="Maximum number of laboratories that will be held"
                    prepend-icon="mdi-numeric"
                    :error-messages="errors"
                    color="black"
                    required
                  />
                </validation-provider>
              </v-flex>
              <v-flex xs12 md4>
                <validation-provider
                  name="maximum number of seminaries"
                  v-slot="{ errors }"
                  :rules="rules.between_0_30"
                >
                  <v-text-field
                    v-model="maxNoSeminaries"
                    type="number"
                    label="Maximum number of seminaries that will be held"
                    prepend-icon="mdi-numeric"
                    :error-messages="errors"
                    color="black"
                    required
                  />
                </validation-provider>
              </v-flex>
            </v-layout>
            <v-layout row class="pa-3">
              <v-flex xs12 md4>
                <validation-provider
                  name="attendance weight"
                  v-slot="{ errors }"
                  :rules="rules.between_0_100"
                >
                  <v-text-field
                    v-model="attendanceImportance"
                    type="number"
                    label="Attendance weight (percentage)"
                    prepend-icon="mdi-numeric"
                    :error-messages="errors"
                    color="black"
                    required
                  />
                </validation-provider>
              </v-flex>
              <v-flex xs12 md4>
                <validation-provider
                  name="bonus points weight"
                  v-slot="{ errors }"
                  :rules="rules.between_0_100"
                >
                  <v-text-field
                    v-model="bonusPointImportance"
                    type="number"
                    label="Bonus points weight (percentage)"
                    prepend-icon="mdi-numeric"
                    :error-messages="errors"
                    color="black"
                    required
                  />
                </validation-provider>
              </v-flex>
            </v-layout>
            <p
              class="red--text"
              v-if="
                computeImportancePercentage != 100 &&
                computeImportancePercentage != 0
              "
            >
              The sum of those two percentage should be 100!!
            </p>
          </v-layout>
          <v-layout row align-center column="4">
            <v-btn
              class="dark_button white--text ma-2"
              @click="onSubmitAsync"
              :disabled="
                invalid || !selectedCourse || computeImportancePercentage != 100
              "
            >
              Save
            </v-btn>
          </v-layout></v-form
        >
      </validation-observer>
    </v-expansion-panel-content>
  </v-expansion-panel>
</template>

<script lang="ts">
import Vue from "vue";
import { rules } from "@/plugins/vee-validate";
import { CourseViewModule } from "@/modules/view-modules";
import storeHelper from "@/store/store-helper";
import { UpdateReportParameters } from "@/modules/commands-parameters";
import { Toastification } from "@/plugins/vue-toastification";

export default Vue.extend({
  name: "UpdateDocumentInformationsComponent",
  data: function () {
    return {
      /** Rules for validating the fields */
      rules,
      /** Document title */
      documentTitle: storeHelper.documentStore.report.title,
      /** Selected course */
      selectedCourse: storeHelper.courseStore.courses.find(
        (c) => c.courseId == storeHelper.documentStore.report.courseId
      ),
      /** Maximum number of lessons that will be held */
      maxNoLessons: storeHelper.documentStore.report.maxNoLessons,
      /** Maximum number of laboratories that will be held */
      maxNoLaboratories:
        storeHelper.documentStore.report.maxNoLaboratories,
      /** Maximum number of seminaries that will be held */
      maxNoSeminaries:
        storeHelper.documentStore.report.maxNoSeminaries,
      /** The percentage of importance for attendance */
      attendanceImportance:
        storeHelper.documentStore.report.attendanceImportance,
      /** The percentage of importance for bonus points */
      bonusPointImportance:
        storeHelper.documentStore.report.bonusPointsImportance,
    };
  },
  computed: {
    /** List of current user courses */
    courses: function (): CourseViewModule[] {
      return storeHelper.courseStore.courses;
    },
    /** Return the value of those two percentages to be sure that together will be 100 (I need to convert them because are treat as strings)*/
    computeImportancePercentage: function (): number {
      return (
        Number(this.attendanceImportance) + Number(this.bonusPointImportance)
      );
    },
  },
  /** Load the current user informations from the API and also his courses */
  created: async function () {
    await storeHelper.userStore.loadCurrentUserInfoAsync();
    await storeHelper.courseStore.loadCoursesAsync(false);
  },
  methods: {
    /** Update document */
    onSubmitAsync: async function (): Promise<void> {
      if (!this.selectedCourse) {
        return;
      }

      const response = await storeHelper.documentStore.updateReportAsync({
        module: {
          courseId: this.selectedCourse.courseId,
          noLaboratories: this.maxNoLaboratories,
          noLessons: this.maxNoLessons,
          noSeminaries: this.maxNoSeminaries,
          title: this.documentTitle,
          attendanceImportance: this.attendanceImportance,
          bonusPointsImportance: this.bonusPointImportance,
          documentId: storeHelper.documentStore.report.reportId,
        } as UpdateReportParameters,
        newCourseName: this.selectedCourse.name,
      });

      if (response) {
        Toastification.success("The new information was successfully updated!");
      }
    },
  },
});
</script>