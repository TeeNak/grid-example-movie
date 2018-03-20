package com.github.taisyun.hrdemo.domain;

import org.apache.commons.lang3.StringUtils;
import org.apache.commons.lang3.builder.EqualsBuilder;
import org.apache.commons.lang3.builder.HashCodeBuilder;
import org.hibernate.validator.constraints.Length;
import org.hibernate.validator.constraints.NotEmpty;

import javax.persistence.*;

@Entity
@Table(name = "job")
public class Job {

    public static final int MAX_LENGTH_CODE = 10;
    public static final int MAX_LENGTH_NAME = 200;

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
//    @Column(name = "job_id")   // SpringNamingStrategy is the default naming convention
            Integer jobId;

    @Length(max = MAX_LENGTH_CODE)
    @NotEmpty(message = "Code is required.")
    String code;

    @Length(max = MAX_LENGTH_NAME)
    @NotEmpty(message = "Name is required.")
    String name;

    @Version
    Integer version;

    public Integer getJobId() {
        return jobId;
    }

    public void setJobId(Integer jobId) {
        this.jobId = jobId;
    }

    public String getCode() {
        return code;
    }

    public void setCode(String code) {
        this.code = code;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public Integer getVersion() {
        return version;
    }

    public void setVersion(Integer version) {
        this.version = version;
    }

    @Override
    public String toString() {
        return StringUtils.join(getName(), ":", getCode());
    }

    @Override
    public int hashCode() {
        return HashCodeBuilder.reflectionHashCode(this);
    }

    @Override
    public boolean equals(Object obj) {
        return EqualsBuilder.reflectionEquals(this, obj);
    }


    public static class Builder {

        private Job built;

        public Builder() {
            built = new Job();
        }

        public Job build() {
            return built;
        }

        public Builder jobId(Integer jobId) {
            built.jobId = jobId;
            return this;
        }

        public Builder code(String code) {
            built.code = code;
            return this;
        }

        public Builder name(String name) {
            built.name = name;
            return this;
        }

        public Builder version(Integer version) {
            built.version = version;
            return this;
        }

    }
}
