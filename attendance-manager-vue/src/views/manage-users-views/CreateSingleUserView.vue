
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
                rules="required"
                name="fullname"
                v-slot="{ errors }"
              >
                <v-text-field
                  label="Fullname"
                  v-model="fullname"
                  prepend-icon="mdi-account"
                  :error-messages="errors"
                  class="pa-6"
                  required
                />
              </validation-provider>
              <validation-provider
                rules="required|email"
                name="email"
                v-slot="{ errors }"
              >
                <v-text-field
                  label="Email"
                  v-model="email"
                  prepend-icon="mdi-email"
                  :error-messages="errors"
                  class="pa-6"
                  required
                />
              </validation-provider>
              <validation-provider
                rules="required"
                name="selectedDepartment"
                v-slot="{ errors }"
              >
                <v-select
                  :items="departments"
                  label="Department"
                  :error-messages="errors"
                  prepend-icon="mdi-folder-open"
                  @change="onFillSpecializations"
                  class="pa-6"
                  item-text="name"
                  item-value="id"
                  ref="departmentRef"
                  required
                ></v-select>
              </validation-provider>
              <validation-provider
                rules="required"
                name="selectedSpecializations"
                v-slot="{ errors }"
              >
                <v-select
                  :items="specializations"
                  label="Specializations"
                  :error-messages="errors"
                  v-model="selectedSpecializations"
                  prepend-icon="mdi-file"
                  class="pa-6"
                  item-text="name"
                  item-value="id"
                  :disabled="specializations.length == 0"
                  attach
                  chips
                  multiple
                  required
                  v-if="role == 2"
                ></v-select>
                <v-select
                  :items="specializations"
                  label="Specializations"
                  :error-messages="errors"
                  v-model="selectedSpecializations"
                  prepend-icon="mdi-file"
                  class="pa-6"
                  item-text="name"
                  item-value="id"
                  :disabled="specializations.length == 0"
                  required
                  v-else
                ></v-select>
              </validation-provider>
              <v-row justify="center">
                <v-col cols="6">
                  <validation-provider
                    rules="required"
                    name="GDPR code"
                    v-slot="{ errors }"
                  >
                    <v-text-field
                      label="GDPR Code"
                      v-model="code"
                      prepend-icon="mdi-account"
                      :error-messages="errors"
                      class="pa-6"
                      required
                    />
                  </validation-provider>
                </v-col>
                <v-col cols="6">
                  <validation-provider
                    name="year"
                    v-slot="{ errors }"
                    rules="required"
                  >
                    <v-select
                      :items="years"
                      label="Enrollment year"
                      v-model="year"
                      required
                      :error-messages="errors"
                      prepend-icon="mdi-school"
                      class="pa-6"
                    ></v-select>
                  </validation-provider>
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
                <v-btn width="50%" @click="onSubmit" :disabled="invalid" large
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
import { extend } from "vee-validate";
import { required, email, min } from "vee-validate/dist/rules";
import { CreateUserParameters } from "@/modules/user";
import { DepartmentModule } from "@/modules/organization/departments";
import storeHelper from "@/store/store-helper";
import { SpecializationModule } from "@/modules/organization/specializations";

/**
 * Validation for requied
 */
extend("required", {
  ...required,
  message: "{_field_} can not be empty",
});

/**
 * Validation for minimum length
 */
extend("min", {
  ...min,
  message: "{_field_} must be greater than {length} characters",
});

/**
 * Validation for email
 */
extend("email", {
  ...email,
  message: "Email must be valid",
});

export default Vue.extend({
  data() {
    return {
      // Email
      email: "",
      //Fullname
      fullname: "",
      //GDPR code
      code: "",
      //user's role; Default value is 1 => student
      role: 1,
      // Enroll year
      year: "",
      // All the specializations
      specializations: [] as SpecializationModule[],
      // Selected specializations
      selectedSpecializations: []
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
     * Get departments to fill the v-selector
     */
    departments(): DepartmentModule[] {
      return storeHelper.organizationStore.departments;
    },
  },
  methods: {
    /**
     * Use this method to add a new user then inform the admin about the process
     */
    async onSubmit(): Promise<void> {
      const response = await storeHelper.userStore.addUser({
        fullname: this.fullname,
        code: this.code,
        email: this.email,
        role: this.role.toString(),
        year: this.year,
        specializations: this.selectedSpecializations,//Array.map({)
      } as CreateUserParameters);

      if (response.isSuccess) {
        this.fullname = "";
        this.email = "";
        this.code = "";
        this.year = "";
        this.role = 1;
        this.specializations = [];
        this.selectedSpecializations = [];
        this.$refs.observer?.reset();
        this.$refs.departmentRef?.reset();
        window.alert(
          "The user was created and he will be inform via email regarding his credentials."
        );
      }
    },

    /**
     * Get the list with all specializations by department id
     * @param selectedDepartment
     */
    onFillSpecializations(selectedDepartment): void {
      this.specializations = storeHelper.organizationStore.specializations(selectedDepartment);
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
    