<template>
  <v-container>
    <v-card>
      <v-card-title>
        <span class="text-h5"> Add new attendances document</span>
      </v-card-title>
      <v-card-text>
        <v-container>
          <v-stepper v-model="currentStep">
            <!--Headers-->
            <v-stepper-header>
              <v-stepper-step :complete="currentStep > 1" step="1">
                Document details
              </v-stepper-step>
              <v-divider></v-divider>
              <v-stepper-step :complete="currentStep > 2" step="2">
                Students
              </v-stepper-step>
              <v-divider></v-divider>
              <v-stepper-step step="3"> Finish </v-stepper-step>
            </v-stepper-header>

            <!--Stept1: document details-->
            <v-stepper-items>
              <v-stepper-content step="1">
                <validation-observer ref="observer" v-slot="{ invalid }">
                  <v-row>
                    <v-col cols="12">
                      <validation-provider
                        name="documentTitle"
                        v-slot="{ errors }"
                        rules="required"
                      >
                        <v-text-field
                          v-model="documentTitle"
                          type="text"
                          label="Document title"
                          prepend-icon="mdi-pencil"
                          :error-messages="errors"
                          required
                        />
                      </validation-provider>
                    </v-col>
                    <v-col cols="12">
                      <validation-provider
                        name="selectedCourse"
                        v-slot="{ errors }"
                        rules="required"
                      >
                        <v-select
                          :items="courses"
                          label="Course"
                          :error-messages="errors"
                          v-model="selectedCourse"
                          required
                          prepend-icon="mdi-school"
                          item-text="name"
                          item-value="id"
                        ></v-select>
                      </validation-provider>
                    </v-col>
                    <v-col cols="4">
                      <validation-provider
                        name="maxNoLessons"
                        v-slot="{ errors }"
                        rules="required"
                      >
                        <v-text-field
                          v-model="maxNoLessons"
                          type="number"
                          label="Maximum number of lessons that will be held "
                          prepend-icon="mdi-pencil"
                          :error-messages="errors"
                          required
                        />
                      </validation-provider>
                    </v-col>
                    <v-col cols="4">
                      <validation-provider
                        name="maxNoLaboratories"
                        v-slot="{ errors }"
                        rules="required"
                      >
                        <v-text-field
                          v-model="maxNoLaboratories"
                          type="number"
                          label="Maximum number of laboratories that will be held"
                          prepend-icon="mdi-pencil"
                          :error-messages="errors"
                          required
                        />
                      </validation-provider>
                    </v-col>
                    <v-col cols="4">
                      <validation-provider
                        name="maxNoSeminaries"
                        v-slot="{ errors }"
                        rules="required"
                      >
                        <v-text-field
                          v-model="maxNoSeminaries"
                          type="number"
                          label="Maximum number of seminaries that will be held"
                          prepend-icon="mdi-pencil"
                          :error-messages="errors"
                          required
                        />
                      </validation-provider>
                    </v-col>
                  </v-row>

                  <v-btn
                    color="primary"
                    @click="currentStep = 2"
                    :disabled="invalid"
                  >
                    Continue
                  </v-btn>
                </validation-observer>
              </v-stepper-content>

              <v-stepper-content step="2">
                <validation-observer ref="observer" v-slot="{ invalid }">
                  <v-row>
                    <v-col cols="12">
                      <validation-provider
                        rules="required"
                        name="selectedSpecialization"
                        v-slot="{ errors }"
                      >
                        <v-select
                          :items="specializations"
                          label="Specialization"
                          :error-messages="errors"
                          v-model="selectedSpecialization"
                          prepend-icon="mdi-file"
                          item-text="name"
                          item-value="id"
                          required
                        ></v-select>
                      </validation-provider>
                    </v-col>
                    <v-col cols="12">
                      <validation-provider
                        name="selectedYear"
                        v-slot="{ errors }"
                        rules="required"
                      >
                        <v-select
                          :items="years"
                          label="Enrollment year"
                          v-model="selectedYear"
                          required
                          :error-messages="errors"
                          prepend-icon="mdi-school"
                        ></v-select>
                      </validation-provider>
                    </v-col>
                  </v-row>

                  <v-btn
                    color="primary"
                    @click="step3Actions"
                    :disabled="invalid"
                  >
                    Continue
                  </v-btn>
                  <v-btn color="primary" @click="currentStep = 1"> Back </v-btn>
                </validation-observer>
              </v-stepper-content>

              <v-stepper-content step="3">
                <v-card class="ma-3">
                  <v-container v-if="students.length == 0">
                    <p class="pa-6">
                      There is no student available. Try another year and
                      specialization.
                    </p>
                  </v-container>
                  <v-container v-else>
                    <v-list>
                      <v-list-item-group
                        v-model="selectedStudents"
                        active-class="blue--text"
                        item-value="email"
                        multiple
                      >
                          <v-list-item v-for="item in students" :key="item.email">
                            <template v-slot:default="{ active }">
                              <v-list-item-content>
                                <v-list-item-title
                                  v-text="item.fullname"
                                ></v-list-item-title>

                                <v-list-item-subtitle
                                  class="text--primary"
                                  v-text="item.email"
                                ></v-list-item-subtitle>
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
                    </v-list>
                  </v-container>
                </v-card>

                <v-btn color="blue darken-1" @click="onSubmit" :disabled="selectedStudents.length==0" text> Create document </v-btn>
                <v-btn color="primary" @click="currentStep = 2"> Back </v-btn>
              </v-stepper-content>
            </v-stepper-items>
          </v-stepper>
        </v-container>
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn color="blue darken-1" text> Close </v-btn>
      </v-card-actions>
    </v-card>
  </v-container>
</template>

<script lang="ts">
import { CourseViewModule } from "@/modules/course";
import { SpecializationModule } from "@/modules/specialization";
import AuthService from "@/services/auth.service";
import {DocumentInsertModule} from "@/modules/document";
import { StudentForCourseViewModule } from "@/modules/user";
import storeHelper from "@/store/store-helper";
import Vue from "vue";
import UserService from "@/services/user.service";
import { ResponseModule } from "@/shared/modules";
import DocumentService from "@/services/document.service";

export default Vue.extend({
  data() {
    return {
      currentStep: 1,
      // Document title
      documentTitle: "",
      // Year when the course is available
      selectedYear: "",
      // List of current user courses
      courses: [] as CourseViewModule[],
      // Selected course
      selectedCourse: "",
      // Selected specializations
      selectedSpecialization: "",
      // User specializations
      specializations: [] as SpecializationModule[],
      // Maximum number of lessons that will be held
      maxNoLessons: "",
      // Maximum number of laboratories that will be held
      maxNoLaboratories: "",
      // Maximum number of seminaries that will be held
      maxNoSeminaries: "",
      generateStudentsList: true,
      students: [] as StudentForCourseViewModule[],
      selectedStudents: [] as number[]
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
  },
  async created() {
    var token = AuthService.getDataFromToken();
    this.specializations = (
      await storeHelper.userStore.loadCurrentUserInfo(token.email)
    ).specializations;
    this.courses = await storeHelper.courseStore.loadCourses(token.email);
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
          //this.selectedStudents = this.students.map(user=>user.email);
          this.selectedStudents = Array.from(Array(this.students.length).keys())
      }
      this.currentStep = 3;
    },
    async onSubmit(): Promise<void> {
      var token = AuthService.getDataFromToken();
      var students: string[] = [];
      this.selectedStudents.forEach(index =>{
        students.push(this.students[index].email);
      })
      
      let newDocument = {
        email: token.email,
        courseId: this.selectedCourse,
        enrollmentYear: this.selectedYear,
        maxNoLaboratories: this.maxNoLaboratories,
        maxNoLessons: this.maxNoLessons,
        maxNoSeminaries: this.maxNoSeminaries,
        title: this.documentTitle,
        specializationId: this.selectedSpecialization,
        studentIds: students
      } as DocumentInsertModule;

      const response = (await DocumentService.addDocument(newDocument)) as ResponseModule;

      if (response.isSuccess) {
        this.$router.push({name:'documents'});
      } else {
        window.alert(response.error);
      }
    }
  },
});
</script>