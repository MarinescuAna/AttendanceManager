
<template>
  <v-container>
    <v-row justify="center">
      <v-card width="50%" class="orange lighten-3">
        <v-card-title class="pa-7">
          <h2>Create single user</h2>
        </v-card-title>
        <v-card-text>
          <validation-observer
            ref="observer"
            v-slot="{ handleSubmit, invalid }"
          >
            <v-form @submit.prevent="handleSubmit(onSubmit)">
              <validation-provider
                :rules="rules.required"
                name="fullname"
                v-slot="{ errors }"
              >
                <v-text-field
                  label="Fullname"
                  v-model="fullname"
                  counter
                  maxlength="128"
                  prepend-icon="mdi-account"
                  :error-messages="errors"
                  class="pa-6"
                  required
                />
              </validation-provider>
              <validation-provider
                :rules="rules.email"
                name="email"
                v-slot="{ errors }"
              >
                <v-text-field
                  label="Email"
                  v-model="email"
                  counter
                  maxlength="254"
                  prepend-icon="mdi-email"
                  :error-messages="errors"
                  class="pa-6"
                  required
                />
              </validation-provider>
              <v-select
                :items="departments"
                label="Department"
                prepend-icon="mdi-folder-open"
                @change="onFillSpecializations"
                class="pa-6"
                item-text="name"
                item-value="id"
                ref="departmentRef"
                required
              ></v-select>
              <v-select
                :items="specializations"
                label="Specializations"
                v-model="selectedSpecializations"
                prepend-icon="mdi-file"
                class="pa-6"
                item-text="name"
                item-value="id"
                :disabled="specializations.length == 0"
                attach
                chips
                deletable-chips
                multiple
                required
                v-if="role == 2"
              ></v-select>
              <v-select
                :items="specializations"
                label="Specializations"
                v-model="selectedSpecialization"
                prepend-icon="mdi-file"
                class="pa-6"
                item-text="name"
                item-value="id"
                :disabled="specializations.length == 0"
                required
                v-else
              ></v-select>
              <v-row justify="center">
                <v-col cols="6">
                  <validation-provider
                    :rules="rules.required"
                    name="GDPR code"
                    v-slot="{ errors }"
                  >
                    <v-text-field
                      label="GDPR Code"
                      v-model="code"
                      counter
                      maxlength="32"
                      prepend-icon="mdi-account"
                      :error-messages="errors"
                      class="pa-6"
                      required
                    />
                  </validation-provider>
                </v-col>
                <v-col cols="6">
                  <v-select
                    :items="years"
                    label="Enrollment year"
                    v-model="year"
                    required
                    prepend-icon="mdi-school"
                    class="pa-6"
                  ></v-select>
                </v-col>
              </v-row>
              <v-container class="ml-3">
                <label class=""><v-icon>mdi-pencil</v-icon> Role</label>
                <v-radio-group
                  v-model="role"
                  class="ml-4"
                  @change="onResetSpecializationSelector"
                >
                  <v-radio label="Student" :value="1"></v-radio>
                  <v-radio label="Teacher" :value="2"></v-radio>
                </v-radio-group>
              </v-container>
              <v-row justify="center" class="pa-8">
                <v-btn
                  width="50%"
                  @click="onSubmit"
                  :disabled="
                    invalid ||
                    (selectedSpecializations.length === 0 && role === 2) ||
                    (selectedSpecialization === 0 && role === 1) ||
                    year === 0
                  "
                  large
                  >Submit</v-btn
                >
              </v-row>
            </v-form>
          </validation-observer>
        </v-card-text>
      </v-card></v-row
    >
  </v-container>
</template>
    
    <script lang="ts">
import Vue from "vue";
import { rules } from "@/plugins/vee-validate";
import { CreateUserParameters } from "@/modules/user";
import storeHelper from "@/store/store-helper";
import { SpecializationViewModule } from "@/modules/specialization";
import { DepartmentViewModule } from "@/modules/department";
import { Role } from "@/shared/enums";
import { Toastification } from "@/plugins/vue-toastification";

export default Vue.extend({
  name: "CreateUserComponent",
  data() {
    return {
      rules,
      // Email
      email: "",
      //Fullname
      fullname: "",
      //GDPR code
      code: "",
      //user's role; Default value is 1 => student
      role: 1,
      // Enroll year
      year: 0,
      // All the specializations
      specializations: [] as SpecializationViewModule[],
      // Selected specializations
      selectedSpecializations: [] as number[],
      // Selected specialization if just one specialization can be selected
      selectedSpecialization: 0,
    };
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
    /**
     * All departments
     */
    departments(): DepartmentViewModule[] {
      return storeHelper.departmentStore.departments;
    },
  },
  methods: {
    /**
     * Use this method to add a new user then inform the admin about the process
     */
    async onSubmit(): Promise<void> {
      let specializationIds: number[] = [];
      if (this.role == Role.Teacher) {
        specializationIds = this.selectedSpecializations;
      } else {
        specializationIds.push(this.selectedSpecialization);
      }

      const department = storeHelper.departmentStore.departments.find(
        (d) => d.id == this.specializations[0].departmentId
      );

      const response = await storeHelper.userStore.addUser(
        {
          fullname: this.fullname,
          code: this.code,
          email: this.email,
          role: this.role.toString(),
          year: this.year,
          specializationIds: specializationIds,
        } as CreateUserParameters,
        department!
      );

      if (response) {
        Toastification.info(
          "The user was created and he will be inform via email regarding his credentials."
        );
        this.$router.push({ name: "users" });
      }
    },

    /**
     * Get the list with all specializations by department id
     * @param selectedDepartment
     */
    onFillSpecializations(selectedDepartment: number): void {
      this.specializations =
        storeHelper.specializationStore.specializations.filter(
          (specialization) => specialization.departmentId == selectedDepartment
        );
      this.selectedSpecializations = [];
    },

    /**
     * Reset specialization v-selector when the role is changed
     */
    onResetSpecializationSelector(): void {
      this.selectedSpecializations = [];
    },
  },
});
</script>
      