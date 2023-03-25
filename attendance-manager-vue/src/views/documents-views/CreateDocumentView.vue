<template>
  <v-layout column>
    <v-flex class="my-3">
      <span class="text-h5 black--text font-weight-bold">
        Add new attendances document</span
      >
    </v-flex>
    <v-flex >
      <v-stepper v-model="currentStep">
        <!--Headers-->
        <v-stepper-header class="header-color">
          <v-stepper-step color="black" :complete="currentStep > 1" step="1">
            Document details
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
                <v-flex>
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
                </v-flex>
                <v-flex>
                  <v-select
                    :items="courses"
                    label="Course"
                    v-model="selectedCourse"
                    required
                    prepend-icon="mdi-school"
                    item-text="name"
                    item-value="id"
                    color="black"
                  ></v-select>
                </v-flex>
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
              </v-layout>
              <v-layout row align-end column="4">
                <v-flex class="my-3 mr-4" align-self-end>
                  <v-btn
                    class="white--text"
                    color="black"
                    @click="currentStep = 2"
                    :disabled="invalid || selectedCourse === 0"
                  >
                    <v-icon>mdi-arrow-right</v-icon>
                  </v-btn>
                </v-flex>
              </v-layout>
            </validation-observer>
          </v-stepper-content>

          <v-stepper-content step="2">
            <v-layout column>
              <v-flex>
                <v-select
                  :items="specializations"
                  label="Specialization"
                  v-model="selectedSpecialization"
                  prepend-icon="mdi-file"
                  item-text="name"
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
                <v-btn
                  class="white--text"
                  color="black"
                  @click="currentStep = 1"
                >
                  <v-icon>mdi-arrow-left</v-icon>
                </v-btn>
                <v-btn
                  class="white--text"
                  color="black"
                  @click="step3Actions"
                  :disabled="selectedSpecialization === 0 || selectedYear === 0"
                >
                  <v-icon>mdi-arrow-right</v-icon>
                </v-btn>
              </v-flex>
            </v-layout>
          </v-stepper-content>

          <v-stepper-content step="3">
            <v-layout v-if="students.length == 0">
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
                          >{{ item.fullname }}</v-list-item-title>

                          <v-list-item-subtitle
                            class="black--text"
                          >{{ item.email }}</v-list-item-subtitle>
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
                  color="black"
                  class="white--text"
                  @click="onSubmit"
                  :disabled="selectedStudents.length == 0"
                >
                  Create document
                </v-btn>
              </v-flex>
            </v-layout>
          </v-stepper-content>
        </v-stepper-items>
      </v-stepper>
    </v-flex>
  </v-layout>
</template>

<style scoped>
.header-color{
  background-color: #FFB74D;
}
</style>

<script lang="ts">
import { CourseViewModule } from "@/modules/course";
import { SpecializationModule } from "@/modules/specialization";
import { DocumentInsertModule } from "@/modules/document";
import { StudentForCourseViewModule } from "@/modules/user";
import storeHelper from "@/store/store-helper";
import Vue from "vue";
import UserService from "@/services/user.service";
import DocumentService from "@/services/document.service";
import { rules } from "@/plugins/vee-validate";

export default Vue.extend({
  name: "CreateDocumentView",
  data() {
    return {
      rules,
      currentStep: 1,
      // Document title
      documentTitle: "",
      // Year when the course is available
      selectedYear: 0,
      // Selected course
      selectedCourse: 0,
      // Selected specializations
      selectedSpecialization: 0,
      // Maximum number of lessons that will be held
      maxNoLessons: 0,
      // Maximum number of laboratories that will be held
      maxNoLaboratories: 0,
      // Maximum number of seminaries that will be held
      maxNoSeminaries: 0,
      generateStudentsList: true,
      students: [] as StudentForCourseViewModule[],
      selectedStudents: [] as number[],
    };
  },
  watch: {
    selectedSpecialization(): void {
      this.resetStudentsList();
    },
    selectedYear(): void {
      this.resetStudentsList();
    },
  },
  computed: {
    /**
     * Get all the years between the current year and 1950
     */
    years(): string[] {
      return Array.from(Array(new Date().getFullYear() - 1949), (_, i) =>
        (new Date().getFullYear() - i).toString()
      );
    },
    // User specializations
    specializations(): SpecializationModule[] {
      return storeHelper.userStore.currentUser.specializations;
    },
    // List of current user courses
    courses(): CourseViewModule[] {
      return storeHelper.courseStore.courses;
    },
  },
  async created() {
    await storeHelper.userStore.loadCurrentUserInfo();
    await storeHelper.courseStore.loadCourses();
  },
  methods: {
    resetStudentsList(): void {
      this.generateStudentsList = true;
      if (this.students.length > 0) {
        this.students.splice(0);
        this.selectedStudents.splice(0);
      }
    },
    async step3Actions(): Promise<void> {
      if (this.generateStudentsList) {
        this.students =
          await UserService.getStudentsBySpecializationIdEnrollmentYear(
            this.selectedYear,
            this.selectedSpecialization
          );
        this.selectedStudents = Array.from(Array(this.students.length).keys());
      }
      this.currentStep = 3;
    },
    async onSubmit(): Promise<void> {
      const response = await DocumentService.addDocument({
        courseId: this.selectedCourse,
        enrollmentYear: this.selectedYear,
        maxNoLaboratories: this.maxNoLaboratories,
        maxNoLessons: this.maxNoLessons,
        maxNoSeminaries: this.maxNoSeminaries,
        title: this.documentTitle,
        specializationId: this.selectedSpecialization,
        studentIds: this.selectedStudents.map((x) => this.students[x].email),
      } as DocumentInsertModule);

      if (response) {
        this.$router.push({ name: "created-documents" });
      }
    },
  },
});
</script>