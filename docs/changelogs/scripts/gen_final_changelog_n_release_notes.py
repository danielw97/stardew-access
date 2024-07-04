import argparse
import re
import os
import shutil
import json
import copy_changelogs_to


def main():
    version, release_notes_path, detailed_release_notes = get_version_n_output_file_name_from_cli()

    if version == 'auto':
        version = get_version_from_manifest()

    final_changelog_path = f'../{version}.md'

    print(f'Version: {version}')
    print(f'Final changelog file path: {final_changelog_path}')
    print(f'Release notes path: {final_changelog_path}\n')
    print('Generating final changelog...')

    gen_final_changelog_file(final_changelog_path,
                             ('beta' in version or 'alpha' in version))
    print('Generating release notes...')
    gen_release_notes(release_notes_path, final_changelog_path,
                      detailed_release_notes, version)


def gen_release_notes(release_notes_path: str,
                      from_path: str,
                      detailed: bool,
                      version: str):
    print(f'Detailed release notes generation: {detailed}')
    release_notes_file = open(release_notes_path, 'w')
    release_notes = []
    changelogs_dict = copy_changelogs_to.get_changelogs_dict(from_path)
    for heading, changelogs in changelogs_dict.items():
        if not detailed and heading != '### New Features' and heading != '### Feature Updates':
            continue
        if heading == '### Translation Changes':
            continue
        if len(changelogs) == 0:
            continue

        print(f'Copying changelogs with heading: {heading}')
        release_notes += [heading, ''] + changelogs + ['']

    print(f'Adding in links to full changelogs and translation changes...')
    changelog_link = f'https://github.com/khanshoaib3/stardew-access/blob/development/docs/changelogs/{version}.md'
    release_notes = ['## Changelog', ''] + release_notes
    release_notes += ['', f'Translators please refer to this link for a list of translation changes: {changelog_link}#translation-changes']
    release_notes += [f'Full changelog at: {changelog_link}']
    release_notes = [f'{line}\n' for line in release_notes]  # Add trailing line break
    release_notes_file.writelines(release_notes)
    release_notes_file.close()


def gen_final_changelog_file(final_changelog_path: str, is_pre_release: bool):
    latest_file = '../latest.md'
    default_file = '../default.md'
    open(final_changelog_path, 'w').close()  # Creates an empty file

    if is_pre_release:
        print(f'Copying contents of latest.md directly as the version is pre release...')
        shutil.copyfile(latest_file, final_changelog_path)
        shutil.copyfile(default_file, latest_file)
        return

    # Merge changelogs from betas and alphas into the final changelog
    patt: str = r"v[0-9]+\.[0-9]+\.[0-9]+.*\.md"
    versions_paths = [f'../{f}' for f in os.listdir('../')
                      if re.match(patt, f) and ('beta' in f or 'alpha' in f)]
    shutil.copyfile(default_file, final_changelog_path)
    print(f'Merging contents of latest.md with following file:', *versions_paths)

    for versions_path in versions_paths:
        print(f'Copying contents of and then removing {versions_path}...')
        copy_changelogs_to.copy_changelog(versions_path, final_changelog_path)
        os.remove(versions_path)

    print(f'Copying contents of latest.md...')
    copy_changelogs_to.copy_changelog(latest_file, final_changelog_path)

    print('Resetting latest.md...')
    shutil.copyfile(default_file, latest_file)


def get_version_from_manifest() -> str:
    manifest_json_file = open('../../../stardew-access/manifest.json')
    manifest_json = json.load(manifest_json_file)
    manifest_json_file.close()
    return f"v{manifest_json['Version']}"


def get_version_n_output_file_name_from_cli():
    parser = argparse.ArgumentParser()
    parser.add_argument('-v', '--version', default='auto')
    parser.add_argument('-o', '--output', default='../temp_notes.md')
    parser.add_argument('-d', '--detailed', action='store_true')

    parsed_args = parser.parse_args()
    return [parsed_args.version, parsed_args.output, parsed_args.detailed]


if __name__ == "__main__":
    main()
