<template>
  <v-expansion-panel>
    <v-expansion-panel-header class="font-weight-bold"
      >Update the document information</v-expansion-panel-header
    >
    <v-expansion-panel-content>
      <validation-observer ref="observer" v-slot="{ invalid, handleSubmit }">
        <v-form @submit.prevent="handleSubmit(onSubmit)">
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
                label="Document title"
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
              item-value="id"
              color="black"
              return-object
            ></v-select>
            <v-layout row class="pa-3">
              <v-flex xs12 md4>
                <validation-provider
                  name="maximum number of lessons"
                  v-slot="{ errors }"
                  :rules="rules.between_0_30"
                >
                  <v-text-field
                    v-model="maxNoLessons"
                    type="number"
                    label="Maximum number of lessons that will be held "
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
                  name="attendances importance percentage"
                  v-slot="{ errors }"
                  :rules="rules.between_0_100"
                >
                  <v-text-field
                    v-model="attendanceImportance"
                    type="number"
                    label="The percentage of attandance importance"
                    prepend-icon="mdi-numeric"
                    :error-messages="errors"
                    color="black"
                    required
                  />
                </validation-provider>
              </v-flex>
              <v-flex xs12 md4>
                <validation-provider
                  name="bonus points importance percentage"
                  v-slot="{ errors }"
                  :rules="rules.between_0_100"
                >
                  <v-text-field
                    v-model="bonusPointImportance"
                    type="number"
                    label="The percentage of bonus points importance"
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
              class="white--text ma-2"
              color="black"
              @click="onSubmit"
              :disabled="
                invalid || !selectedCourse || computeImportancePercentage != 100
              "
            >
              Update new data
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
import { CourseViewModule } from "@/modules/course";
import storeHelper from "@/store/store-helper";
import { DocumentUpdateModule } from "@/modules/document";
import { Toastification } from "@/plugins/vue-toastification";

export default Vue.extend({
  name: "UpdateDocumentInformationsComponent",
  data: function () {
    return {
      /** Rules for validating the fields */
      rules,
      /** Document title */
      documentTitle: storeHelper.documentStore.documentDetails.title,
      /** Selected course */
      selectedCourse: storeHelper.courseStore.courses.find(
        (c) => c.id == storeHelper.documentStore.documentDetails.courseId
      ),
      /** Maximum number of lessons that will be held */
      maxNoLessons: storeHelper.documentStore.documentDetails.maxNoLessons,
      /** Maximum number of laboratories that will be held */
      maxNoLaboratories:
        storeHelper.documentStore.documentDetails.maxNoLaboratories,
      /** Maximum number of seminaries that will be held */
      maxNoSeminaries:
        storeHelper.documentStore.documentDetails.maxNoSeminaries,
      /** The percentage of importance for attendances */
      attendanceImportance:
        storeHelper.documentStore.documentDetails.attendanceImportance,
      /** The percentage of importance for bonus points */
      bonusPointImportance:
        storeHelper.documentStore.documentDetails.bonusPointsImportance,
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
    await storeHelper.userStore.loadCurrentUserInfo();
    await storeHelper.courseStore.loadCourses();
  },
  methods: {
    /** Update document */
    onSubmit: async function (): Promise<void> {
      if (!this.selectedCourse) {
        return;
      }

      const response = await storeHelper.documentStore.updateDocument({
        module: {
          courseId: this.selectedCourse.id,
          noLaboratories: this.maxNoLaboratories,
          noLessons: this.maxNoLessons,
          noSeminaries: this.maxNoSeminaries,
          title: this.documentTitle,
          attendanceImportance: this.attendanceImportance,
          bonusPointsImportance: this.bonusPointImportance,
          documentId: storeHelper.documentStore.documentDetails.documentId,
        } as DocumentUpdateModule,
        newCourseName: this.selectedCourse.name,
      });

      if (response) {
        Toastification.success("The new information was successfully updated!");
      }
    },
  },
});
</script>