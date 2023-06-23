<template>
  <v-layout column>
    <v-flex class="my-3">
      <span class="text-h5 black--text font-weight-bold">
        Add new involvement report</span
      >
    </v-flex>
    <v-flex>
      <v-stepper v-model="currentStep">
        <!--Headers-->
        <v-stepper-header class="header-color">
          <v-stepper-step color="black" :complete="currentStep > 1" step="1">
            Involvement report details
          </v-stepper-step>
          <v-divider></v-divider>
          <v-stepper-step color="black" :complete="currentStep > 2" step="2">
            Students
          </v-stepper-step>
          <v-divider></v-divider>
          <v-stepper-step color="black" step="3"> Finish </v-stepper-step>
        </v-stepper-header>

        <!--Stept1: document details-->
        <v-stepper-items>
          <v-stepper-content step="1">
            <validation-observer ref="observer" v-slot="{ invalid }">
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
                    label="Report title"
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
                  return-object
                  prepend-icon="mdi-school"
                  item-text="name"
                  item-value="courseId"
                  color="black"
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
                      name="attendances importance percentage"
                      v-slot="{ errors }"
                      :rules="rules.between_0_100"
                    >
                      <v-text-field
                        v-model="attendanceImportance"
                        type="number"
                        label="The percentage of attandance weight"
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
                        label="The percentage of bonus points weight"
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
              <v-layout row align-end column="4">
                <v-flex class="my-3 mr-4" align-self-end>
                  <v-btn
                    class="dark_button white--text"
                    @click="currentStep = 2"
                    :disabled="
                      invalid ||
                      Object.keys(selectedCourse).length === 0 ||
                      computeImportancePercentage != 100
                    "
                  >
                    <v-icon>mdi-arrow-right</v-icon>
                  </v-btn>
                </v-flex>
              </v-layout>
            </validation-observer>
          </v-stepper-content>

          <v-stepper-content step="2">
            <v-layout v-if="currentStep == 2" column>
              <v-flex>
                <v-select
                  :items="specializations"
                  label="Specialization"
                  v-model="selectedSpecialization"
                  prepend-icon="mdi-file"
                  item-text="name"
                  return-object
                  item-value="id"
                  color="black"
                  required
                ></v-select>
              </v-flex>
              <v-flex>
                <v-select
                  :items="years"
                  label="Enrollment year"
                  v-model="selectedYear"
                  required
                  color="black"
                  prepend-icon="mdi-school"
                ></v-select>
              </v-flex>
            </v-layout>
            <v-layout row align-end column="4" class="my-3 mr-4">
              <v-flex>
                <v-btn class="dark_button white--text" @click="currentStep = 1">
                  <v-icon>mdi-arrow-left</v-icon>
                </v-btn>
                <v-btn
                  class="dark_button white--text"
                  @click="step3ActionsAsync"
                  :disabled="
                    Object.keys(selectedSpecialization).length === 0 ||
                    selectedYear === 0
                  "
                >
                  <v-icon>mdi-arrow-right</v-icon>
                </v-btn>
              </v-flex>
            </v-layout>
          </v-stepper-content>

          <v-stepper-content step="3">
            <v-layout v-if="students.length == 0 && currentStep == 3">
              <p class="pa-6">
                There is no student available. Try another year and
                specialization.
              </p>
            </v-layout>
            <v-layout column="12" v-else>
              <v-flex>
                <v-list>
                  <v-list-item-group
                    v-model="selectedStudents"
                    active-class="black--text"
                    item-value="email"
                    multiple
                  >
                    <v-list-item v-for="item in students" :key="item.email">
                      <template v-slot:default="{ active }">
                        <v-list-item-content>
                          <v-list-item-title
                            class="black--text font-weight-bold"
                            >{{ item.fullname }}</v-list-item-title
                          >

                          <v-list-item-subtitle class="black--text">{{
                            item.email
                          }}</v-list-item-subtitle>
                        </v-list-item-content>
                        <v-list-item-action>
                          <v-icon v-if="!active" color="grey lighten-1">
                            mdi-star-outline
                          </v-icon>
                          <v-icon v-else color="yellow darken-3">
                            mdi-star
                          </v-icon>
                        </v-list-item-action>
                      </template>
                    </v-list-item>
                  </v-list-item-group>
                </v-list></v-flex
              >
            </v-layout>
            <v-layout row align-end column="4" class="my-3 mr-4">
              <v-flex>
                <v-btn dark @click="currentStep = 2">
                  <v-icon>mdi-arrow-left</v-icon>
                </v-btn>
                <v-btn
                  class="dark_button white--text"
                  @click="onSubmitAsync"
                  :disabled="selectedStudents.length == 0"
                >
                  Create involvement report
                </v-btn>
              </v-flex>
            </v-layout>
          </v-stepper-content>
        </v-stepper-items>
      </v-stepper>
    </v-flex>
  </v-layout>
</template>

<style lang="scss">
.header-color {
  background-color: #ffb74d;
}
</style>

<script lang="ts">
import {
  CourseViewModule,
  SpecializationViewModule,
  StudentForCourseViewModule,
} from "@/modules/view-modules";
import storeHelper from "@/store/store-helper";
import Vue from "vue";
import UserService from "@/services/user.service";
import { rules } from "@/plugins/vee-validate";
import ReportService from "@/services/report.service";
import { InsertReportParameters } from "@/modules/commands-parameters";

export default Vue.extend({
  name: "CreateDocumentView",
  data: function () {
    return {
      /** Rules for validating the fields */
      rules,
      /** Variable for keeping the current stat of the process of creating a new document */
      currentStep: 1,
      /** Document title */
      documentTitle: "",
      /** Year when the course is held */
      selectedYear: 0,
      /** Selected course */
      selectedCourse: {} as CourseViewModule,
      /** Selected specialization */
      selectedSpecialization: {} as SpecializationViewModule,
      /** Maximum number of lessons that will be held */
      maxNoLessons: 0,
      /** Maximum number of laboratories that will be held */
      maxNoLaboratories: 0,
      /** Maximum number of seminaries that will be held */
      maxNoSeminaries: 0,
      /** Boolean value for generating the list of students */
      generateStudentsList: true,
      /** Array of all students that are under selected specialization and selected year */
      students: [] as StudentForCourseViewModule[],
      /** Array of selected students from the list above */
      selectedStudents: [] as number[],
      /** The percentage of importance for attendances */
      attendanceImportance: 0,
      /** The percentage of importance for bonus points */
      bonusPointImportance: 0,
    };
  },
  watch: {
    /** When the selected specialization has changed, reset the list of the students */
    selectedSpecialization: function (): void {
      this.resetStudentsList();
    },
    /** When the selected year has changed, reset the list of the students */
    selectedYear: function (): void {
      this.resetStudentsList();
    },
  },
  computed: {
    /** Get all the years between the current year and 1950 */
    years: function (): string[] {
      return Array.from(Array(new Date().getFullYear() - 1949), (_, i) =>
        (new Date().getFullYear() - i).toString()
      );
    },
    /** Load the current user specializations from the store */
    specializations: function (): SpecializationViewModule[] {
      return storeHelper.userStore.currentUser.specializations;
    },
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
    /** Use this function to reset the list of students when the specialization or year has changed */
    resetStudentsList: function (): void {
      this.generateStudentsList = true;
      if (this.students.length > 0) {
        this.students.splice(0);
        this.selectedStudents.splice(0);
      }
    },
    /** Use this function to load the list of students when we go to the last step */
    step3ActionsAsync: async function (): Promise<void> {
      if (this.generateStudentsList) {
        this.students =
          await UserService.getStudentsBySpecializationIdEnrollmentYearAsync(
            this.selectedYear,
            this.selectedSpecialization.id
          );
        this.selectedStudents = Array.from(Array(this.students.length).keys());
      }
      this.currentStep = 3;
    },
    /** Create the new document according to the selected data, and if any error occures, redirect the user to the created documents list */
    onSubmitAsync: async function (): Promise<void> {
      const response = await ReportService.addReportAsync({
        courseId: this.selectedCourse.courseId,
        enrollmentYear: this.selectedYear,
        maxNoLaboratories: this.maxNoLaboratories,
        maxNoLessons: this.maxNoLessons,
        maxNoSeminaries: this.maxNoSeminaries,
        title: this.documentTitle,
        specializationId: this.selectedSpecialization.id,
        studentIds: this.selectedStudents.map((x) => this.students[x].email),
        attendanceImportance: this.attendanceImportance,
        bonusPointsImportance: this.bonusPointImportance,
      } as InsertReportParameters);

      if (response) {
        this.$router.push({ name: "documents" });
      }
    },
  },
});
</script>