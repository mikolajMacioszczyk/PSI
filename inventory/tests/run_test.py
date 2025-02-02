import pytest
import os


def run_tests():
    test_folders = ['functional', 'integration', 'unit']

    test_files = []
    for folder in test_folders:
        test_files.extend(
            [os.path.join(folder, f) for f in os.listdir(folder) if f.startswith('test_') and f.endswith('.py')])

    pytest.main(test_files + ["-p", "no:warnings"])

if __name__ == "__main__":
    run_tests()

